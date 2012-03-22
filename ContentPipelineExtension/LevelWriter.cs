using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

// TODO: replace this with the type you want to write out.
using TWrite = TowerDefenseXNA.Level;

namespace ContentPipelineExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class LevelWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.playerLife);
            output.Write(value.playerMoney);
            output.Write(value.tilesPerRow);
            output.WriteObject<Queue<Vector2>>(value.waypoints);
            output.Write(value.tileSize);
            output.Write(value.start);
            output.Write(value.end);
            output.WriteObject<int[][]>(value.map);
            output.WriteObject<int[][]>(value.wavelist);

        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SharedData.LevelReader).AssemblyQualifiedName;  
        }
    }
}
