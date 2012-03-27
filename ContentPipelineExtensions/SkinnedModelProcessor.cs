using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DigitalRune.Animation.Character;
using DigitalRune.Mathematics.Algebra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;


namespace ContentPipelineExtensions
{
  // This content processor extends the XNA ModelProcessor to support animations.
  //
  // The SkinnedEffect is used for skeletal animation at runtime. Other effects are replaced 
  // by the SkinnedEffect. (The SkinnedEffect is used as the DefaultEffect.)
  //
  // Merging Animations:
  // Animations of different files can be merged into this asset. The property MergeAnimations
  // can be used to specify a list of file names. See file AnimationMerger.cs
  //
  // Splitting Animations:
  // The animation of the imported file can be split into several separate animations.
  // The property SplitAnimationsFile can be used to specify an XML file that defines the
  // splits. See file AnimationSplitter.cs
  // 
  // Animation Compression:
  // The animations can be compressed (see properties Compress*Threshold).
  //
  // The processed animation information is stored in a dictionary in Model.Tag. 
  // The dictionary contains the following entries:
  //   - "Skeleton" ..... A Skeleton instance.
  //   - "Animations" ... A dictionary of SkeletonKeyFrameAnimations.
  [ContentProcessor(DisplayName = "Skinned Model Processor (DigitalRune Samples)")]
  public class SkinnedModelProcessor : ModelProcessor
  {
    //--------------------------------------------------------------
    #region Properties
    //-------------------------------------------------------------- 

    [DefaultValue(MaterialProcessorDefaultEffect.SkinnedEffect)]
    public override MaterialProcessorDefaultEffect DefaultEffect
    {
      get { return MaterialProcessorDefaultEffect.SkinnedEffect; }
      set { }
    }


    [DisplayName("Merge Animations")]
    [Description("Merge several animations into this file. List the file names relative to the folder of the model file separated by ';'. Example: \"run.fbx;jump.fbx;turn.fbx\"")]
    [DefaultValue(typeof(string), "")]
    public string MergeAnimations
    {
      get { return _mergeAnimations; }
      set { _mergeAnimations = value; }
    }
    private string _mergeAnimations = string.Empty;


    [DisplayName("Split Animations File")]
    [Description("Split animation into separate animations. Specify the file name of the split definition XML file relative to the folder of the model file. For example: \"Dude_AnimationSplits.xml\"")]
    [DefaultValue(typeof(string), "")]
    public string SplitAnimationsFile
    {
      get { return _splitAnimationsFile; }
      set { _splitAnimationsFile = value; }
    }
    private string _splitAnimationsFile = string.Empty;


    [DisplayName("Compression - Translation Threshold")]
    [Description("Define the allowed translation error for key frame compression, for example 0.001. Set to -1 to disable compression.")]
    [DefaultValue(typeof(float), "-1")]
    public float CompressionTranslationThreshold
    {
      get { return _compressionTranslationThreshold; }
      set { _compressionTranslationThreshold = value; }
    }
    private float _compressionTranslationThreshold = -1;


    [DisplayName("Compression - Rotation Threshold")]
    [Description("Define the allowed rotation error after key frame compression (in degrees), for example 0.01. Set to -1 to disable compression.")]
    [DefaultValue(typeof(float), "-1")]
    public float CompressionRotationThreshold
    {
      get { return _compressionRotationThreshold; }
      set { _compressionRotationThreshold = value; }
    }
    private float _compressionRotationThreshold = -1;


    [DisplayName("Compression - Scale Threshold")]
    [Description("Define the allowed scale error after key frame compression, for example 0.01. Set to -1 to disable compression.")]
    [DefaultValue(typeof(float), "-1")]
    public float CompressionScaleThreshold
    {
      get { return _compressionScaleThreshold; }
      set { _compressionScaleThreshold = value; }
    }
    private float _compressionScaleThreshold = -1;
    #endregion


    //--------------------------------------------------------------
    #region Methods
    //--------------------------------------------------------------

    public override ModelContent Process(NodeContent input, ContentProcessorContext context)
    {
      // Uncomment this to launch attach and launch a debugger.
      //System.Diagnostics.Debugger.Launch();

      ValidateSkinnedMesh(input, context, null);

      BoneContent rootNode = MeshHelper.FindSkeleton(input);
      if (rootNode == null)
        throw new InvalidContentException("Skeleton not found.", input.Identity);

      // Merge animations from other files into this file.
      AnimationMerger.Merge(MergeAnimations, rootNode.Animations, input.Identity, context);

      // Split animation into separate animations based on a split definition XML file.
      AnimationSplitter.Split(SplitAnimationsFile, rootNode.Animations, input.Identity, context);

      // Flatten the node transforms - except the skeleton nodes.
      BakeTransforms(input, rootNode);

      // Call the base class to process the model. Note: This can scale or rotate the model, 
      // therefore this must be called before ProcessSkeleton.
      ModelContent model = base.Process(input, context);

      // Extract the skeleton information.
      Skeleton skeleton = ProcessSkeleton(rootNode, context);

      // Extract animations.
      Dictionary<string, SkeletonKeyFrameAnimation> animations = ProcessAnimations(
        rootNode.Animations, skeleton, context);

      // The animation information is stored in a dictionary in Model.Tag.
      Dictionary<string, object> additionalData = new Dictionary<string, object>();
      additionalData.Add("Skeleton", skeleton);
      additionalData.Add("Animations", animations);
      model.Tag = additionalData;

      return model;
    }


    private static void ValidateSkinnedMesh(NodeContent node, ContentProcessorContext context, string parentBoneName)
    {
      var mesh = node as MeshContent;
      if (mesh != null)
      {
        // ----- Check 1: Meshes must not be under a bone.
        if (parentBoneName != null)
        {
          context.Logger.LogWarning(
            null,
            null,
            "Mesh {0} is a child of bone {1}. SkinnedModelProcessor does not correctly handle meshes that are children of bones.",
            mesh.Name,
            parentBoneName);
        }

        // ----- Check 2: If the mesh is not skinned, we ignore it.
        // (A mesh uses skinning if it has animation weights.)
        bool useSkinning = mesh.Geometry.All(geometry => geometry.Vertices.Channels.Contains(VertexChannelNames.Weights()));
        if (!useSkinning)
        {
          context.Logger.LogWarning(
            null,
            null,
            "Mesh {0} has no skinning information, so it has been deleted.",
            mesh.Name);

          mesh.Parent.Children.Remove(mesh);
          return;
        }
      }
      else if (node is BoneContent)
      {
        // If this is a bone, remember that we are now looking into its children.
        parentBoneName = node.Name;
      }

      // ----- Check 3: Check children for warnings and remove not-skinned meshes.
      foreach (NodeContent child in new List<NodeContent>(node.Children))
        ValidateSkinnedMesh(child, context, parentBoneName);
    }


    private static void BakeTransforms(NodeContent node, BoneContent skeleton)
    {
      foreach (NodeContent child in node.Children)
      {
        // Don't process the skeleton, because that is special.
        if (child == skeleton)
          continue;

        MeshHelper.TransformScene(child, child.Transform);
        child.Transform = Matrix.Identity;

        BakeTransforms(child, skeleton);
      }
    }


    protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context)
    {
      if (material is SkinnedMaterialContent)
        return base.ConvertMaterial(material, context);

      // ----- The material is not a SkinnedMaterialContent --> Convert it to SkinnedMaterialContent.

      var skinnedMaterial = new SkinnedMaterialContent();

      //  Copy all textures.
      foreach (KeyValuePair<string, ExternalReference<TextureContent>> item in material.Textures)
        skinnedMaterial.Textures.Add(item.Key, item.Value);

      // Copy properties required by the SkinnedEffect.
      object data;
      if (material.OpaqueData.TryGetValue(SkinnedMaterialContent.AlphaKey, out data))
        skinnedMaterial.Alpha = (float)data;

      if (material.OpaqueData.TryGetValue(SkinnedMaterialContent.DiffuseColorKey, out data))
        skinnedMaterial.DiffuseColor = (Vector3)data;

      if (material.OpaqueData.TryGetValue(SkinnedMaterialContent.EmissiveColorKey, out data))
        skinnedMaterial.EmissiveColor = (Vector3)data;

      if (material.OpaqueData.TryGetValue(SkinnedMaterialContent.SpecularColorKey, out data))
        skinnedMaterial.SpecularColor = (Vector3)data;

      if (material.OpaqueData.TryGetValue(SkinnedMaterialContent.SpecularPowerKey, out data) && data is float)
        skinnedMaterial.SpecularPower = (float)data;

      if (material.OpaqueData.TryGetValue(SkinnedMaterialContent.TextureKey, out data))
      {
        skinnedMaterial.Texture = (ExternalReference<TextureContent>)data;
      }
      else
      {
        // OpaqueData does not contain a texture. --> Use the first available texture instead.
        if (skinnedMaterial.Textures.Count > 0)
          skinnedMaterial.Texture = skinnedMaterial.Textures.Values.First();
      }

      if (material.OpaqueData.TryGetValue(SkinnedMaterialContent.WeightsPerVertexKey, out data))
        skinnedMaterial.WeightsPerVertex = (int)data;

      material = skinnedMaterial;

      return base.ConvertMaterial(material, context);
    }


    protected override void ProcessVertexChannel(GeometryContent geometry, int vertexChannelIndex, ContentProcessorContext context)
    {
      bool isWeights = geometry.Vertices.Channels[vertexChannelIndex].Name == VertexChannelNames.Weights();

      base.ProcessVertexChannel(geometry, vertexChannelIndex, context);

      if (isWeights)
      {
        geometry.Vertices.Channels.ConvertChannelContent<Vector4>("BlendIndices0");
        geometry.Vertices.Channels.ConvertChannelContent<Vector4>("BlendWeight0");
      }
    }


    /// <summary>
    /// Creates a Skeleton from the specified bone content.
    /// </summary>
    private Skeleton ProcessSkeleton(BoneContent rootNode, ContentProcessorContext context)
    {
      // Get all bones as a list (FlattenSkeleton makes sure that the indices are the same as
      // the indices that will be used in the blend index channel).
      IList<BoneContent> bones = MeshHelper.FlattenSkeleton(rootNode);
      if (bones.Count > SkinnedEffect.MaxBones)
      {
        var message = string.Format("Skeleton has {0} bones, but the maximum supported is {1}.", bones.Count, SkinnedEffect.MaxBones);
        throw new InvalidContentException(message, rootNode.Identity);
      }

      // Create list of parent indices, bind pose transformations and bone names.
      List<int> boneParents = new List<int>();
      List<SrtTransform> bindTransforms = new List<SrtTransform>();
      List<string> boneNames = new List<string>();
      foreach (BoneContent bone in bones)
      {
        int parentIndex = bones.IndexOf(bone.Parent as BoneContent);
        boneParents.Add(parentIndex);

        if (!SrtTransform.IsValid((Matrix44F)bone.Transform))
          context.Logger.LogWarning(null, null, "Bone transform is not supported. Bone transform matrices may only contain scaling, rotation and translation.");

        bindTransforms.Add(SrtTransform.FromMatrix(bone.Transform));
        boneNames.Add(bone.Name);
      }

      // Create and return a new skeleton instance.
      return new Skeleton(boneParents, boneNames, bindTransforms);
    }


    /// <summary>
    /// Extracts all animations and stores them in a dictionary of timelines.
    /// </summary>
    private Dictionary<string, SkeletonKeyFrameAnimation> ProcessAnimations(AnimationContentDictionary animationContentDictionary, Skeleton skeleton, ContentProcessorContext context)
    {
      var animations = new Dictionary<string, SkeletonKeyFrameAnimation>();
      foreach (var item in animationContentDictionary)
      {
        string animationName = item.Key;
        AnimationContent animationContent = item.Value;

        // Convert the AnimationContent to a SkeletonKeyFrameAnimation.
        SkeletonKeyFrameAnimation skeletonAnimation = ProcessAnimation(animationContent, skeleton, context);
        animations.Add(animationName, skeletonAnimation);
      }

      if (animations.Count == 0)
        context.Logger.LogWarning(null, null, "Skinned model does not contain any animations.");

      return animations;
    }


    /// <summary>
    /// Converts an AnimationContent to a SkeletonKeyFrameAnimation.
    /// </summary>
    private SkeletonKeyFrameAnimation ProcessAnimation(AnimationContent animationContent, Skeleton skeleton, ContentProcessorContext context)
    {
      var animation = new SkeletonKeyFrameAnimation { EnableInterpolation = true };

      // Process all animation channels (each channel animates a bone).
      int numberOfKeyFrames = 0;
      foreach (var item in animationContent.Channels)
      {
        string channelName = item.Key;
        AnimationChannel channel = item.Value;

        int boneIndex = skeleton.GetIndex(channelName);
        if (boneIndex == -1)
        {
          var message = string.Format("Found animation for bone '{0}', which is not part of the skeleton.", channelName);
          throw new InvalidContentException(message, animationContent.Identity);
        }

        var bindPoseRelativeInverse = skeleton.GetBindPoseRelative(boneIndex).Inverse;
        foreach (AnimationKeyframe keyframe in channel)
        {
          TimeSpan time = keyframe.Time;
          SrtTransform transform = SrtTransform.FromMatrix(keyframe.Transform);

          // The matrix in the key frame is the transformation in the coordinate space of the
          // parent bone. --> Convert it to a transformation relative to the animated bone.
          transform = bindPoseRelativeInverse * transform;

          // To start with minimal numerical errors, we normalize the rotation quaternion.
          transform.Rotation.Normalize();

          animation.AddKeyFrame(boneIndex, time, transform);
          numberOfKeyFrames++;
        }
      }

      if (numberOfKeyFrames == 0)
        throw new InvalidContentException("Animation has no keyframes.", animationContent.Identity);

     // Compress animation to safe memory.
      float removedKeyFrames = animation.Compress(
        CompressionScaleThreshold,
        CompressionRotationThreshold,
        CompressionTranslationThreshold);

      if (removedKeyFrames > 0)
        context.Logger.LogImportantMessage("{0}: Compression removed {1:P} of all key frames.",
                                           animationContent.Name,
                                           removedKeyFrames);

      // Finalize the animation. (Optimizes the animation data for fast runtime access.)
      animation.Freeze();

      return animation;
    }
    #endregion
  }
}