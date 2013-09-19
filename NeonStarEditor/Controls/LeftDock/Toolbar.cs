using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;
using System.Reflection;
using NeonEngine.Private;

namespace NeonStarEditor.Controls.LeftDock
{
    public partial class Toolbar : UserControl
    {
        public EditorScreen GameWorld;

        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Toolbar));

        public Toolbar()
        {
            InitializeComponent();
            this.PausePlayButton.BackgroundImage = this.PausePlayButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PauseButton")));
        }

        protected override void OnLoad(EventArgs e)
        {
            if (GameWorld == null)
            {
                Console.WriteLine("GameWorld is missing in Toolbar.cs");
            }
            base.OnLoad(e);
        }

        private void SaveCurrentMap_Click(object sender, EventArgs e)
        {
            DataManager.SaveLevel(GameWorld, GameWorld.levelFilePath);
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            this.GameWorld.ReloadLevel();
        }

        private void PausePlayButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.Pause)
            {
                GameWorld.Pause = false;
                this.PausePlayButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PauseButton")));
            }
            else
            {
                GameWorld.Pause = true;
                this.PausePlayButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PlayButton")));
            }
        }

        private void Selection_Click(object sender, EventArgs e)
        {
            GameWorld.CurrentTool = new Selection(GameWorld);
        }

        private void ReloadScript_Click(object sender, EventArgs e)
        {
            Neon.NeonScripting.CompileScripts();
            if (Neon.Scripts != null)
                foreach (Entity ent in GameWorld.entities)
                    for (int i = ent.Components.Count - 1; i >= 0; i--)
                    {
                        ScriptComponent sc = ent.Components[i] is ScriptComponent ? (ScriptComponent)ent.Components[i] : null;
                        if (sc != null)
                        {
                            sc.Remove();
                            foreach (Type t in Neon.Scripts)
                                if (sc.GetType().Name == t.Name)
                                    ent.AddComponent((NeonEngine.Component)Activator.CreateInstance(t, ent));
                        }
                    }

            List<Type> Components = new List<Type>(Neon.utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(NeonEngine.Component)) && !(t.IsAbstract)));
            if (Neon.Scripts != null)
                Components.AddRange(Neon.Scripts);
            Components.Add(typeof(ScriptComponent));
            GameWorld.RightDockControl.InspectorControl.ComponentList.DataSource = Components;
            GameWorld.RightDockControl.InspectorControl.ComponentList.DisplayMember = "Name";
        }

        private void CreateRectangle_Click(object sender, EventArgs e)
        {
            GameWorld.CurrentTool = new CreateRectangle(GameWorld);
        }

        private void ReloadAssetsButton_Click(object sender, EventArgs e)
        {
            AssetManager.LoadAssets(GameWorld.game.GraphicsDevice);
        }
    }
}
