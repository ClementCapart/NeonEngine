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
using Component = NeonEngine.Component;
using NeonStarLibrary;

namespace NeonStarEditor.Controls.LeftDock
{
    public partial class Toolbar : UserControl
    {
        public EditorScreen GameWorld;

        ComponentResourceManager resources = new ComponentResourceManager(typeof(Toolbar));

        public Toolbar()
        {
            InitializeComponent();
            PausePlayButton.BackgroundImage = PausePlayButton.BackgroundImage = ((Image)(resources.GetObject("PauseButton")));
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
            GameWorld.ReloadLevel();
        }

        private void PausePlayButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.Pause)
            {
                GameWorld.Pause = false;
                PausePlayButton.BackgroundImage = ((Image)(resources.GetObject("PauseButton")));
            }
            else
            {
                GameWorld.Pause = true;
                PausePlayButton.BackgroundImage = ((Image)(resources.GetObject("PlayButton")));
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
                                    ent.AddComponent((Component)Activator.CreateInstance(t, ent));
                        }
                    }

            List<Type> Components = new List<Type>(Neon.utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(Component)) && !(t.IsAbstract)));
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

        private void AttackManagerButton_Click(object sender, EventArgs e)
        {
            GameWorld.ToggleAttackManager();
        }

        private void CameraPanel_Click(object sender, EventArgs e)
        {
            GameWorld.ToggleCameraManager();
        }

        private void ToggleBoundsButton_Click(object sender, EventArgs e)
        {
            GameWorld._boundsGizmoToggled = !GameWorld._boundsGizmoToggled;
        }

        private void ToggleLightButton_Click(object sender, EventArgs e)
        {
            GameWorld._lightGizmoToggled = !GameWorld._lightGizmoToggled;
        }

        private void PathNodeTool_Click(object sender, EventArgs e)
        {
            GameWorld.TogglePathNodeManager();
        }

        private void magnetCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GameWorld.MagnetismActivated = (sender as CheckBox).Checked;
        }
    }
}
