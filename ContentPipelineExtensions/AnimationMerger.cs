using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;


namespace ContentPipelineExtensions
{
  /// <summary>
  /// Merges the animations of several animation files (e.g. .fbx) into a given NodeContent.
  /// </summary>
  /// <remarks>
  /// See http://blogs.msdn.com/b/shawnhar/archive/2010/06/18/merging-animation-files.aspx for more
  /// information.
  /// </remarks>
  public static class AnimationMerger
  {
    /// <summary>
    /// Merges the specified animation files to the specified animation dictionary.
    /// </summary>
    /// <param name="animationFiles">
    /// The animation files as a string separated by semicolon (relative to the folder of the
    /// model file). For example: "run.fbx;jump.fbx;turn.fbx".
    /// </param>
    /// <param name="animationDictionary">The animation dictionary.</param>
    /// <param name="contentIdentity">The content identity.</param>
    /// <param name="context">The content processor context.</param>
    public static void Merge(string animationFiles, AnimationContentDictionary animationDictionary,
                             ContentIdentity contentIdentity, ContentProcessorContext context)
    {
      if (string.IsNullOrEmpty(animationFiles))
        return;

        // Get path of the model file.
        string sourcePath = Path.GetDirectoryName(contentIdentity.SourceFilename);
        
      var files = animationFiles.Split(';')
                                .Select(s => s.Trim())
                                .Where(s => !string.IsNullOrEmpty(s));
      foreach (string file in files)
        {
          string filePath = Path.GetFullPath(Path.Combine(sourcePath, file));
          MergeAnimation(filePath, animationDictionary, contentIdentity, context);
        }
      }


    private static void MergeAnimation(string animationFilePath, AnimationContentDictionary animationDictionary,
                                       ContentIdentity contentIdentity, ContentProcessorContext context)
    {
      // Use content pipeline to build the asset.
      NodeContent mergeModel = context.BuildAndLoadAsset<NodeContent, NodeContent>(new ExternalReference<NodeContent>(animationFilePath), null);

      // Find the skeleton.
      BoneContent mergeRoot = MeshHelper.FindSkeleton(mergeModel);
      if (mergeRoot == null)
      {
        context.Logger.LogWarning(null, contentIdentity, "Animation model file '{0}' has no root bone. Cannot merge animations.", animationFilePath);
        return;
      }

      // Merge all animations of the skeleton root node.
      foreach (string animationName in mergeRoot.Animations.Keys)
      {
        if (animationDictionary.ContainsKey(animationName))
        {
          context.Logger.LogWarning(null, contentIdentity,
              "Replacing animation '{0}' from '{1}' with merged animation.",
              animationName, animationFilePath);

          animationDictionary[animationName] = mergeRoot.Animations[animationName];
        }
        else
        {
          context.Logger.LogImportantMessage("Merging animation '{0}' from '{1}'.", animationName, animationFilePath);

          animationDictionary.Add(animationName, mergeRoot.Animations[animationName]);  
        }
      }
    }
  }
}