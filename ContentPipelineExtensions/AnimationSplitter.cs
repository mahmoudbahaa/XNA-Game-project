using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;


namespace ContentPipelineExtensions
{
  /// <summary>
  /// Splits an animation into separate animations based on a XML file defining the splits.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Some FBX exporters support only a single animation (take) per model. One solution to support 
  /// multiple animations per model is to concatenate all animations into a single long one. A 
  /// separate, manually created XML file defines the animation sections.
  /// </para>
  /// <example>
  /// The XML file allows to specify the start and end times in "seconds" or in "frames". The file 
  /// must use this format:
  /// <code lang="xml">
  /// <![CDATA[
  /// <?xml version="1.0" encoding="utf-8"?>
  /// <Animations>
  ///   <!-- Needed if you want to specify start and end in frames. -->
  ///   <Framerate>30</Framerate>
  /// 
  ///   <!-- Using time in seconds: -->
  ///   <Animation Name="Walk" StartTime="0" EndTime="1.5"/>
  /// 
  ///   <!-- Or using frames: -->
  ///   <Animation Name="Walk2" StartFrame="45" EndFrame="60"/>
  /// </Animations>
  /// ]]>
  /// </code>
  /// </example>
  /// <para>
  /// The method <see cref="Split"/> parses the XML split definition file, removes the original 
  /// animation and cuts it into separate animations.
  /// </para>
  /// </remarks>
  public static class AnimationSplitter
  {
    /// <summary>
    /// Defines a region in the original (merged) animation.
    /// </summary>
    private class SplitDefinition
    {
      /// <summary>
      /// The name of the animation.
      /// </summary>
      public string Name;
      
      /// <summary>
      /// The start time.
      /// </summary>
      public TimeSpan StartTime;

      /// <summary>
      /// The end time.
      /// </summary>
      public TimeSpan EndTime;
    }


    /// <summary>
    /// Splits the animation in the specified animation dictionary into several separate animations.
    /// </summary>
    /// <param name="splitFile">
    /// The path of the XML file defining the splits. This path is relative to the folder of 
    /// the model file. Usually it is simply the filename, e.g. "Dude_AnimationSplits.xml".
    /// </param>
    /// <param name="animationDictionary">The animation dictionary.</param>
    /// <param name="contentIdentity">The content identity.</param>
    /// <param name="context">The content processor context.</param>
    public static void Split(string splitFile, AnimationContentDictionary animationDictionary, 
                             ContentIdentity contentIdentity, ContentProcessorContext context)
    {
      // ----- Check input parameters.
      if (string.IsNullOrEmpty(splitFile))
        return;
      
      if (animationDictionary == null)
        return;
      
      if (contentIdentity == null)
        throw new ArgumentNullException("contentIdentity");
      
      if (context == null)
        throw new ArgumentNullException("context");

      if (animationDictionary.Count == 0)
      {
        context.Logger.LogWarning(null, contentIdentity, "The model does not have an animation. Animation splitting is skipped.");
        return;
      }

      if (animationDictionary.Count > 1)
        context.Logger.LogWarning(null, contentIdentity, "The model contains more than 1 animation. The animation splitting is performed on the first animation. Other animations are deleted!");

      // ----- Parse the XML file.
      List<SplitDefinition> splits = ParseSplitAnimationFile(splitFile, contentIdentity, context);
      if (splits == null)
      {
        context.Logger.LogWarning(null, contentIdentity, "The XML file with the animation split definitions was not found. Animation is not split.");
        return;
      }
      if (splits.Count == 0)
      {
        context.Logger.LogWarning(null, contentIdentity, "The XML file with the animation split definitions is invalid or empty. Animation is not split.");
        return;
      }

      // Get first animation.
      var originalAnimation = animationDictionary.First().Value;

      // Clear animation dictionary. - We do not keep the original animations!
      animationDictionary.Clear();

      // Add an animation to animationDictionary for each split.
      foreach (var split in splits)
      {
        TimeSpan startTime = split.StartTime;
        TimeSpan endTime = split.EndTime;

        var newAnimation = new AnimationContent
        {
          Duration = endTime - startTime
        };

        // Process all channels.
        foreach (var item in originalAnimation.Channels)
        {
          string channelName = item.Key;
          AnimationChannel originalChannel = item.Value;
          if (originalChannel.Count == 0)
            return;

          AnimationChannel newChannel = new AnimationChannel();

          // Add all key frames to the channel that are in the split interval.
          foreach (AnimationKeyframe keyFrame in originalChannel)
          {
            TimeSpan time = keyFrame.Time;
            if (startTime <= time && time <= endTime)
            {
              newChannel.Add(new AnimationKeyframe(keyFrame.Time - startTime, keyFrame.Transform));
            }
          }

          // Add channel if it contains key frames.
          if (newChannel.Count > 0)
            newAnimation.Channels.Add(channelName, newChannel);
        }

        if (newAnimation.Channels.Count == 0)
        {
          var message = string.Format("The split animation '{0}' is empty.", split.Name);
          throw new InvalidContentException(message, contentIdentity);
        }

        if (animationDictionary.ContainsKey(split.Name))
        {
          var message = string.Format("Cannot add split animation '{0}' because an animation with the same name already exits.", split.Name);
          throw new InvalidContentException(message, contentIdentity);
        }

        animationDictionary.Add(split.Name, newAnimation);
      }
    }


    // Parses the XML file defining the splits.
    // This method returns null if the file is not found.
    // This method returns an empty collection if the file does not contain any split definitions or 
    // if the format is invalid.
    private static List<SplitDefinition> ParseSplitAnimationFile(string splitFile, ContentIdentity contentIdentity, ContentProcessorContext context)
    {
      // Get full path of XML file. File location is relative to the location of the model.
      string sourcePath = Path.GetDirectoryName(contentIdentity.SourceFilename);
      string splitFilePath = Path.GetFullPath(Path.Combine(sourcePath, splitFile));
      XDocument document;
      try
      {
         document = XDocument.Load(splitFilePath);
      }
      catch (FileNotFoundException)
      {
        return null;
      }

      var splits = new List<SplitDefinition>();

      // Let the content pipeline know that we depend on this file and we need to rebuild the 
      // content if the file is modified.
      context.AddDependency(splitFilePath);

      var animationsNode = document.Element("Animations");
      if (animationsNode == null)
      {
        context.Logger.LogWarning(null, contentIdentity, splitFile + " does not contain an <Animations> root node.");
        return splits;
      }

      double? framerate = (double?)animationsNode.Element("Framerate");

      foreach (var animationNode in animationsNode.Elements("Animation"))
      {
        var name = (string)animationNode.Attribute("Name");
        if (string.IsNullOrEmpty(name))
          throw new InvalidContentException(splitFile + " contains an <Animation> element with a missing or empty 'Name' attribute", contentIdentity);

        double? startTime = (double?)animationNode.Attribute("StartTime");
        double? endTime = (double?)animationNode.Attribute("EndTime");

        var startFrame = (int?)animationNode.Attribute("StartFrame");
        var endFrame = (int?)animationNode.Attribute("EndFrame");

        if (startTime == null && startFrame == null)
        {
          var message = string.Format("{0} contains an <Animation> element that does not contain a valid 'StartTime' or 'StartFrame' attribute.", splitFile);
          throw new InvalidContentException(message, contentIdentity);
        }

        if (endTime == null && endFrame == null)
        {
          var message = string.Format("{0} contains an <Animation> element that does not contain a valid 'EndTime' or 'EndFrame' attribute.", splitFile);
          throw new InvalidContentException(message, contentIdentity);
        }

        if (framerate == null && (startTime == null || endTime == null))
        {
          var message = string.Format("{0} must have a <Framerate> element if start and end time are specified in 'frame' is used.", splitFile);
          throw new InvalidContentException(message, contentIdentity);
        }

        startTime = startTime ?? startFrame.Value / framerate.Value;
        endTime = endTime ?? endFrame.Value / framerate.Value;
        var start = new TimeSpan((long)(startTime.Value * TimeSpan.TicksPerSecond));
        var end = new TimeSpan((long)(endTime.Value * TimeSpan.TicksPerSecond));

        if (start > end)
        {
          var message = string.Format("{0} contains an invalid <Animation> element: The start time is larger than the end time.", splitFile);
          throw new InvalidContentException(message, contentIdentity);
        }

        splits.Add(new SplitDefinition { Name = name, StartTime = start, EndTime = end });
      }

      return splits;
    }
  }
}