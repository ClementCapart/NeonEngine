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
using Microsoft.Xna.Framework.Audio;

namespace NeonStarEditor.Controls
{
    public partial class SoundPickerControl : UserControl
    {
        public EditorScreen GameWorld;

        private PropertyInfo _propertyInfo;
        private NeonEngine.Component _component;
        private Label _label;
        private SoundEffect _selectedSound;
        public SoundEffectInstance PlayingSound;

        public SoundPickerControl(EditorScreen gameWorld, PropertyInfo pi, NeonEngine.Component c, Label l)
        {
            GameWorld = gameWorld;

            _propertyInfo = pi;
            _component = c;
            _label = l;

            InitializeComponent();
            if (GameWorld.SelectedEntity != null)
                this.entityInfo.Text = GameWorld.SelectedEntity.Name + " - " + c.Name + " - " + pi.Name;
            else
                GameWorld.ToggleSoundPicker();
            
            this.Location = new System.Drawing.Point((int)(Neon.HalfScreen.X - this.Width / 2) - 150, (int)(Neon.HalfScreen.Y - this.Height / 2));

            InitializeSoundList();
        }

        public void InitializeSoundList()
        {
            TreeNode noAsset = new TreeNode("NoSound");
            noAsset.Name = "";
            soundList.Nodes.Add(noAsset);

            TreeNode sounds = new TreeNode("Sounds");
            sounds.Name = "Sounds";
            soundList.Nodes.Add(sounds);

            TreeNode nodeToSelect = null;

            foreach (KeyValuePair<string, string> kvp in SoundManager.Sounds)
            {
                string[] folders = kvp.Value.Split('\\');
                TreeNode tn = sounds;

                for (int i = 1; i < folders.Length; i++)
                {
                    if (i == folders.Length - 1)
                    {
                        TreeNode newNode = new TreeNode(kvp.Key);
                        newNode.Name = kvp.Key;
                        newNode.Tag = kvp.Value;
                        tn.Nodes.Add(newNode);
                        tn = newNode;
                        if (_label != null && _label.Text == newNode.Name)
                            nodeToSelect = newNode;
                        continue;
                    }
                    if (!tn.Nodes.ContainsKey(folders[i]))
                    {
                        TreeNode newNode = new TreeNode(folders[i]);
                        newNode.Name = folders[i];
                        tn.Nodes.Add(newNode);
                        tn = newNode;
                    }
                    else
                    {
                        tn = tn.Nodes[folders[i]];
                    }
                }
            }

            if (sounds.Nodes.Count == 0)
                sounds.Remove();

            if (nodeToSelect != null)
            {
                soundList.SelectedNode = nodeToSelect;
                soundList.Select();
            }         
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null && _propertyInfo != null && _component != null && _label != null && soundList.SelectedNode.Nodes.Count == 0)
            {
                _label.Text = soundList.SelectedNode.Name;
                _propertyInfo.SetValue(_component, soundList.SelectedNode.Text, null);
                GameWorld.ToggleSoundPicker();
            }
            else
            {
                Console.WriteLine("Error : Couldn't select this as a Spritesheet");
            }
        }

        private void soundList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (soundList.SelectedNode.Nodes.Count == 0 && soundList.SelectedNode.Tag != null)
            {
                _selectedSound = SoundManager.GetSound(soundList.SelectedNode.Text);
                if (_selectedSound != null)
                {
                    this.Length.Text = _selectedSound.Duration.ToString("mm':'ss':'ffff");
                }
                else
                {
                    Console.WriteLine("Sound couldn't be load");
                    this.Length.Text = "00:00";
                }
            }
            else
            {
                this.Length.Text = "00:00";
                _selectedSound = null;
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (_selectedSound != null)
            {
                if (PlayingSound != null)
                    PlayingSound.Stop();
                PlayingSound = _selectedSound.CreateInstance();
                PlayingSound.Play();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PlayingSound != null)
                PlayingSound.Stop();
        }

        private void PlayLoopButton_Click(object sender, EventArgs e)
        {
            if (_selectedSound != null)
            {
                if (PlayingSound != null)
                    PlayingSound.Stop();
                PlayingSound = _selectedSound.CreateInstance();
                PlayingSound.IsLooped = true;
                PlayingSound.Play();
            }
        }
    }


}
