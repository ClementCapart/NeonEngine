using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework.Graphics;
using NeonStarLibrary;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using System.IO;
using Microsoft.Xna.Framework.Media;
using System.Xml.Linq;
using System.Diagnostics;
using NeonEngine.Components.Camera;
using NeonEngine.Components.VisualFX;
using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.CollisionDetection;
using NeonStarEditor.Controls;

namespace NeonStarEditor
{
    public class EditorScreen : GameScreen
    {
        public bool EntityChangedThisFrame = false;
        public bool DisplayAllPathNodeList = false;
        public bool DisplayAllSpawnPoint = false;

        public bool FlyingModeActivated = false;

        public bool EditorVisible = true;
        public Form GameAsForm;
        public BottomDock BottomDockControl;
        public RightDock RightDockControl;
        public LeftDock LeftDockControl;

        public AttacksSettingsManager AttacksSettingsManager;
        public ElementPanel ElementSettingsManager;
        public PathNodesPanel PathNodePanel;
        public SpawnPointPanel SpawnPointsPanel;
        public AddComponentControl AddComponentPanel;

        public GraphicPickerControl GraphicPicker;
        public SpritesheetPickerControl SpritesheetPicker;

        public Tool CurrentTool;
        public Entity SelectedEntity;

        public bool MagnetismActivated = true;
        public float MagnetismValue = 1.0f;

        public TextBox FocusedTextBox = null;
        public NumericUpDown FocusedNumericUpDown = null;
        float PressedDelay = 0.0f;

        public bool MouseInGameWindow = true;

        public bool FocusEntity = false;
        public bool LockedCamera = true;

        private Texture2D _colorEmitterTexture;
        private Texture2D _colorEmitterCircleTexture;
        private Texture2D _boundTexture;
        private Texture2D _nodeTexture;
        private Texture2D _spawnTexture;

        public GraphicsDeviceManager graphics;
        public bool IsActiveForm = false;
        private bool _isAttackManagerDisplayed = false;
        private bool _isPathNodeManagerDisplayed = false;
        private bool _isSpawnPointsManagerDisplayed = false;
        private bool _isElementManagerDisplayed = false;
        private bool _isAddComponentPanelDisplayed = false;
        private bool _isGraphicPickerDisplayed = false;
        private bool _isSpritesheetPickerDisplayed = false;

        public bool _lightGizmoToggled = false;
        public bool _boundsGizmoToggled = false;

        public bool ManagingInspector = false;

        public bool UnpauseTillNextFrame = false;

        public string DefaultLayer = "";

        public EditorScreen(string groupName, string levelName, int startingSpawnPointIndex, XElement statusToLoad, Game game, GraphicsDeviceManager graphics, bool loadPreferences = false)
            : base(groupName, levelName, startingSpawnPointIndex, statusToLoad, game)
        {
            GameAsForm = Control.FromHandle(this.game.Window.Handle) as Form;
            this.graphics = graphics;
            game.IsMouseVisible = true;
            CurrentTool = new Selection(this);

            //GameAsForm.Controls.Add(new NeonStarToolstrip());       
            
            _colorEmitterTexture = AssetManager.GetTexture("LightBulb");
            _colorEmitterCircleTexture = AssetManager.GetTexture("LightCircle");
            _boundTexture = AssetManager.GetTexture("BoundIcon");
            _nodeTexture = AssetManager.GetTexture("NodeIcon");
            _spawnTexture = AssetManager.GetTexture("SpawnPointIcon");

            LeftDockControl = new LeftDock(this);
            BottomDockControl = new BottomDock(this);
            RightDockControl = new RightDock(this);

            GameAsForm.Controls.Add(BottomDockControl);
            GameAsForm.Controls.Add(RightDockControl);
            GameAsForm.Controls.Add(LeftDockControl);

            

            GameAsForm.MouseEnter += GameAsForm_MouseEnter;
            GameAsForm.MouseLeave += GameAsForm_MouseLeave;
            GameAsForm.KeyPreview = true;

            

            if (loadPreferences)
            {
                try
                {
                    XDocument preferencesFile = XDocument.Load(@"../Data/Config/EditorPreferences.xml");
                    XElement preferences = preferencesFile.Element("XnaContent").Element("Preferences");

                    string levelFilePath = preferences.Element("LevelToLoad").Value;
                    bool showEditor = bool.Parse(preferences.Element("ShowEditor").Value);
                    bool showHitboxes = bool.Parse(preferences.Element("ShowHitboxes").Value);
                    bool showPhysics = bool.Parse(preferences.Element("ShowPhysics").Value);
                    string defaultLayer = preferences.Element("DefaultLayer").Value;

                    if (showEditor)
                    {
                        foreach (Control c in GameAsForm.Controls)
                            c.Show();
                        EditorVisible = true;
                    }
                    else
                    {
                        foreach (Control c in GameAsForm.Controls)
                            c.Hide();
                        EditorVisible = false;
                    }

                    if (showHitboxes)
                        Neon.DrawHitboxes = true;

                    if (showPhysics)
                        Neon.DebugViewEnabled = true;

                    BottomDockControl.levelList.DefaultLayerBox.Text = defaultLayer;
                }
                catch
                {
                    Console.WriteLine("Can't load any preferences file");
                }
            }
            else
            {
                foreach (Control c in GameAsForm.Controls)
                    c.Hide();
                EditorVisible = false;
            }

            RefreshInspector(null);
            BottomDockControl.entityListControl.InitializeEntityList();
        }

        public void ToggleAttackManager()
        {
            
            if (_isAttackManagerDisplayed)
            {
                GameAsForm.Controls.Remove(AttacksSettingsManager);
                _isAttackManagerDisplayed = false;
                AttacksSettingsManager.Dispose();
                AttacksSettingsManager = null;
            }
            else
            {
                if (_isElementManagerDisplayed)
                    ToggleElementPanel();

                if (_isAddComponentPanelDisplayed)
                    ToggleAddComponentPanel();

                AttacksSettingsManager = new AttacksSettingsManager();
                AttacksSettingsManager.InitializeData();
                GameAsForm.Controls.Add(AttacksSettingsManager);
                _isAttackManagerDisplayed = true;
            }
        }


        public void ToggleElementPanel()
        {
            if (_isElementManagerDisplayed)
            {
                GameAsForm.Controls.Remove(ElementSettingsManager);
                _isElementManagerDisplayed = false;
                ElementSettingsManager.Dispose();
                ElementSettingsManager = null;
            }
            else
            {
                if (_isAttackManagerDisplayed)
                    ToggleAttackManager();

                if (_isAddComponentPanelDisplayed)
                    ToggleAddComponentPanel();

                ElementSettingsManager = new ElementPanel();
                ElementSettingsManager.InitializeData();
                GameAsForm.Controls.Add(ElementSettingsManager);
                _isElementManagerDisplayed = true;
            }
        }

        public void TogglePathNodeManager()
        {
            

            if (_isPathNodeManagerDisplayed)
            {
                GameAsForm.Controls.Remove(PathNodePanel);
                PathNodePanel.Dispose();
                this.CurrentTool = new Selection(this);
                PathNodePanel = null;
                _isPathNodeManagerDisplayed = false;
            }
            else
            {
                if (_isSpawnPointsManagerDisplayed)
                    ToggleSpawnPointManager();
                PathNodePanel = new PathNodesPanel(this);
                GameAsForm.Controls.Add(PathNodePanel);
                _isPathNodeManagerDisplayed = true;
            }
        }

        public void ToggleSpawnPointManager()
        {
            if (_isSpawnPointsManagerDisplayed)
            {
                GameAsForm.Controls.Remove(SpawnPointsPanel);
                SpawnPointsPanel.Dispose();
                this.CurrentTool = new Selection(this);
                SpawnPointsPanel = null;
                _isSpawnPointsManagerDisplayed = false;
            }
            else
            {
                if (_isPathNodeManagerDisplayed)
                    TogglePathNodeManager();
                SpawnPointsPanel = new SpawnPointPanel(this);
                this.CurrentTool = new SelectSpawn(this);
                GameAsForm.Controls.Add(SpawnPointsPanel);
                _isSpawnPointsManagerDisplayed = true;
            }
        }

        public void ToggleAddComponentPanel()
        {
            if (_isAddComponentPanelDisplayed)
            {
                GameAsForm.Controls.Remove(AddComponentPanel);
                AddComponentPanel.Dispose();
                AddComponentPanel = null;
                _isAddComponentPanelDisplayed = false;
            }
            else
            {
                if (_isElementManagerDisplayed)
                    ToggleElementPanel();

                if (_isAttackManagerDisplayed)
                    ToggleAttackManager();

                AddComponentPanel = new AddComponentControl(this);
                CurrentTool = new Selection(this);
                GameAsForm.Controls.Add(AddComponentPanel);
                _isAddComponentPanelDisplayed = true;
            }
        }

        public void ToggleGraphicPicker(PropertyInfo pi = null, NeonEngine.Component c = null, Label l = null)
        {
            if (pi == null || c == null || l == null)
            {
                if (_isGraphicPickerDisplayed)
                {
                    GameAsForm.Controls.Remove(GraphicPicker);
                    GraphicPicker.Dispose();
                    GraphicPicker = null;
                    _isGraphicPickerDisplayed = false;
                    return;
                }
                else
                {
                    return;
                }
            }

            if (_isGraphicPickerDisplayed)
            {
                GameAsForm.Controls.Remove(GraphicPicker);
                GraphicPicker.Dispose();
                GraphicPicker = null;
                _isGraphicPickerDisplayed = false;
            }
            else
            {
                if (_isElementManagerDisplayed)
                    ToggleElementPanel();

                if (_isAttackManagerDisplayed)
                    ToggleAttackManager();

                if (_isAddComponentPanelDisplayed)
                    ToggleAddComponentPanel();

                GraphicPicker = new GraphicPickerControl(this, pi, c, l);
                CurrentTool = new Selection(this);
                GameAsForm.Controls.Add(GraphicPicker);
                _isGraphicPickerDisplayed = true;
            }
        }

        public void ToggleSpritesheetPicker(PropertyInfo pi = null, NeonEngine.Component c = null, Label l = null)
        {
            if (pi == null || c == null || l == null)
            {
                if (_isSpritesheetPickerDisplayed)
                {
                    GameAsForm.Controls.Remove(SpritesheetPicker);
                    SpritesheetPicker.Dispose();
                    SpritesheetPicker = null;
                    _isSpritesheetPickerDisplayed = false;
                    return;
                }
                else
                {
                    return;
                }
            }

            if (_isSpritesheetPickerDisplayed)
            {
                GameAsForm.Controls.Remove(SpritesheetPicker);
                SpritesheetPicker.Dispose();
                SpritesheetPicker = null;
                _isSpritesheetPickerDisplayed = false;
            }
            else
            {
                if (_isElementManagerDisplayed)
                    ToggleElementPanel();

                if (_isAttackManagerDisplayed)
                    ToggleAttackManager();

                if (_isAddComponentPanelDisplayed)
                    ToggleAddComponentPanel();

                SpritesheetPicker = new SpritesheetPickerControl(this, pi, c, l);
                CurrentTool = new Selection(this);
                GameAsForm.Controls.Add(SpritesheetPicker);
                _isSpritesheetPickerDisplayed = true;
            }
        }

        public void ToggleSpritesheetPicker(SpritesheetInspector ssi, TextBox tb, Label l)
        {
            if (ssi == null || tb == null || l == null)
            {
                if (_isSpritesheetPickerDisplayed)
                {
                    GameAsForm.Controls.Remove(SpritesheetPicker);
                    SpritesheetPicker.Dispose();
                    SpritesheetPicker = null;
                    _isSpritesheetPickerDisplayed = false;
                    return;
                }
                else
                {
                    return;
                }
            }

            if (_isSpritesheetPickerDisplayed)
            {
                GameAsForm.Controls.Remove(SpritesheetPicker);
                SpritesheetPicker.Dispose();
                SpritesheetPicker = null;
                _isSpritesheetPickerDisplayed = false;
            }
            else
            {
                if (_isElementManagerDisplayed)
                    ToggleElementPanel();

                if (_isAttackManagerDisplayed)
                    ToggleAttackManager();

                if (_isAddComponentPanelDisplayed)
                    ToggleAddComponentPanel();

                SpritesheetPicker = new SpritesheetPickerControl(this, ssi, tb, l);
                CurrentTool = new Selection(this);
                GameAsForm.Controls.Add(SpritesheetPicker);
                _isSpritesheetPickerDisplayed = true;
            }
        }


        void GameAsForm_MouseLeave(object sender, EventArgs e)
        {
            MouseInGameWindow = false;
        }

        void GameAsForm_MouseEnter(object sender, EventArgs e)
        {
            MouseInGameWindow = true;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (!Pause)
                if (UnpauseTillNextFrame)
                {
                    Pause = true;
                    UnpauseTillNextFrame = false;
                }

            if (Neon.Input.Check(Buttons.LeftStick) && Neon.Input.Pressed(Buttons.RightStick))
            {
                if (!FlyingModeActivated)
                {
                    FlyingModeActivated = true;
                    if (avatar != null)
                    {
                        avatar.rigidbody.body.Enabled = false;
                        avatar.hitboxes[0].Type = HitboxType.Invincible;
                        Camera.Bounded = false;
                    }
                }
                else
                {
                    FlyingModeActivated = false;
                    if (avatar != null)
                    {
                        avatar.rigidbody.body.Enabled = true;
                        avatar.hitboxes[0].Type = HitboxType.Main;
                        Camera.Bounded = true;
                    }
                }
            }

            if (FlyingModeActivated && avatar != null)
            {
                int speed = 5;
                if (Neon.Input.Check(Buttons.RightStick))
                    speed = 15;

                if (Neon.Input.Check(Buttons.LeftThumbstickLeft))
                {
                    avatar.transform.Position = new Vector2(avatar.transform.Position.X - speed, avatar.transform.Position.Y);
                }
                else if (Neon.Input.Check(Buttons.LeftThumbstickRight))
                {
                    avatar.transform.Position = new Vector2(avatar.transform.Position.X + speed, avatar.transform.Position.Y);
                }

                if (Neon.Input.Check(Buttons.LeftThumbstickUp))
                {
                    avatar.transform.Position = new Vector2(avatar.transform.Position.X, avatar.transform.Position.Y - speed);
                }
                else if (Neon.Input.Check(Buttons.LeftThumbstickDown))
                {
                    avatar.transform.Position = new Vector2(avatar.transform.Position.X, avatar.transform.Position.Y + speed);
                }
            }

            if (Neon.Input.Pressed(Buttons.Start))
            {
                UnpauseTillNextFrame = false;
            }

            this.IsActiveForm = System.Windows.Forms.Form.ActiveForm == this.GameAsForm;

            if (CurrentTool != null && MouseInGameWindow)
                CurrentTool.Update(gameTime);

            if (Neon.Input.MouseCheck(MouseButton.MiddleButton) && MouseInGameWindow)
            {
                FocusEntity = false;
                MustFollowAvatar = false;
                Camera.Bounded = false;
                Camera.Position += new Vector2(-Neon.Input.DeltaMouse.X, -Neon.Input.DeltaMouse.Y);
            }

            if (Neon.Input.Pressed(Keys.F) && FocusedTextBox == null && FocusedNumericUpDown == null)
                FocusEntity = !FocusEntity;

            if (Neon.Input.Pressed(Keys.H) && FocusedTextBox == null && FocusedNumericUpDown == null)
            {
                if (EditorVisible)
                {
                    foreach (Control c in GameAsForm.Controls)
                        c.Hide();
                    EditorVisible = false;
                }
                else
                {
                    BottomDockControl.entityListControl.InitializeEntityList();

                    foreach (Control c in GameAsForm.Controls)
                        c.Show();
                    EditorVisible = true;
                }
            }

            if (Neon.Input.MouseWheel() > 0)
                Camera.Zoom += 0.1f;
            else if (Neon.Input.MouseWheel() < 0)
                Camera.Zoom -= 0.1f;

            if (Neon.Input.Check(Keys.R) && FocusedTextBox == null && FocusedNumericUpDown == null)
            {
                FocusEntity = false;
                Camera.Bounded = true;
                MustFollowAvatar = true;
                Camera.Zoom = 1f;
            }

            if (FocusEntity && SelectedEntity != null)
            {
                Camera.SmoothFollow(SelectedEntity);
                Camera.Bounded = false;
                MustFollowAvatar = false;
            }

            /*if (((Neon.Input.Pressed(Keys.LeftControl) || Neon.Input.Pressed(Keys.RightControl))&& Neon.Input.Check(Keys.Z))
                || (Neon.Input.Check(Keys.LeftControl) || Neon.Input.Check(Keys.RightControl))&& Neon.Input.Pressed(Keys.Z))
                ActionManager.Undo();*/
            if (PressedDelay > 0.0f)
            {
                PressedDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
                PressedDelay = 0.0f;

            if(CurrentTool == null)
                CurrentTool = new Selection(this);

            base.PostUpdate(gameTime);
            
        }

        public override void PreUpdate(GameTime gameTime)
        {
            ManageInspector();
            EntityChangedThisFrame = false;
            base.PreUpdate(gameTime);
        }

        public override void ManualDrawGame(SpriteBatch spriteBatch)
        {
            if (EditorVisible)
            {
                if (FocusEntity)
                {
                    if (SelectedEntity != null)
                    {
                        DrawableComponent dc = SelectedEntity.GetComponent<DrawableComponent>();
                        if (dc != null)
                        {
                            spriteBatch.End();
                            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
                            spriteBatch.Draw(AssetManager.GetTexture("neon_screen"), Vector2.Zero, Color.Lerp(Color.Transparent, Color.White, 0.7f));
                            spriteBatch.End();
                            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.get_transformation(graphics.GraphicsDevice));
                            dc.Draw(spriteBatch);
                        }
                    }
                }
                else
                {
                    if (_isPathNodeManagerDisplayed)
                    {
                        if (DisplayAllPathNodeList)
                        {
                            foreach (PathNodeList pnl in this.NodeLists)
                            {
                                for (int i = pnl.Nodes.Count - 1; i >= 0; i--)
                                {
                                    Node n = pnl.Nodes[i];

                                    
                                    if (pnl.Nodes.Count > 1)
                                    {
                                        Node nextNode;
                                        if (i == 0)
                                            nextNode = pnl.Nodes[pnl.Nodes.Count - 1];
                                        else
                                            nextNode = pnl.Nodes[i - 1];

                                        PrimitiveLine pl = new PrimitiveLine(Neon.GraphicsDevice);
                                        pl.vectors.Add(n.Position);
                                        pl.Colour = Color.Red;
                                        pl.Depth = 0.6f;
                                        pl.vectors.Add(nextNode.Position);
                                        pl.Render(spriteBatch);
                                    }

                                }

                                foreach (Node n in pnl.Nodes)
                                {
                                        spriteBatch.Draw(_nodeTexture, n.Position, null, PathNodePanel.CurrentNodeSelected == n ? Color.Red :Color.White, 0, new Vector2(_nodeTexture.Width / 2, _nodeTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                                }
                            }
                        }
                        else
                        {
                            if (PathNodePanel.NodeLists.SelectedIndex != -1)
                            {
                                for(int i = NodeLists[PathNodePanel.NodeLists.SelectedIndex].Nodes.Count - 1; i >= 0; i --)
                                {
                                    Node n = NodeLists[PathNodePanel.NodeLists.SelectedIndex].Nodes[i];
                                    
                                    
                                    if(NodeLists[PathNodePanel.NodeLists.SelectedIndex].Nodes.Count > 1)
                                    {
                                        Node nextNode;
                                        if (i == 0)
                                            nextNode = NodeLists[PathNodePanel.NodeLists.SelectedIndex].Nodes[NodeLists[PathNodePanel.NodeLists.SelectedIndex].Nodes.Count - 1];
                                        else
                                            nextNode = NodeLists[PathNodePanel.NodeLists.SelectedIndex].Nodes[i - 1];

                                        PrimitiveLine pl = new PrimitiveLine(Neon.GraphicsDevice);
                                        pl.vectors.Add(n.Position);
                                        pl.Colour = Color.Red;
                                        pl.Depth = 0.6f;
                                        pl.vectors.Add(nextNode.Position);
                                        pl.Render(spriteBatch);
                                    }
                                    
                                }

                                for (int i = NodeLists[PathNodePanel.NodeLists.SelectedIndex].Nodes.Count - 1; i >= 0; i--)
                                {
                                    Node n = NodeLists[PathNodePanel.NodeLists.SelectedIndex].Nodes[i];

                                    spriteBatch.Draw(_nodeTexture, n.Position, null, PathNodePanel.CurrentNodeSelected == n ? Color.Red : Color.White, 0, new Vector2(_nodeTexture.Width / 2, _nodeTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                                }
                            }
                        }
                        
                    }

                    if (_isSpawnPointsManagerDisplayed)
                    {
                        if (DisplayAllSpawnPoint || !DisplayAllSpawnPoint)//May obviously change later if asked
                        {
                            foreach (SpawnPoint sp in SpawnPoints.Where(sp => sp != SpawnPointsPanel.CurrentSpawnPointSelected))
                                spriteBatch.Draw(_spawnTexture, sp.Position, null, Color.White, 0f, new Vector2(_spawnTexture.Width / 2, _spawnTexture.Height / 2), 1f, (sp.Side == Side.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f);

                            if(SpawnPointsPanel.CurrentSpawnPointSelected != null)
                                spriteBatch.Draw(_spawnTexture, SpawnPointsPanel.CurrentSpawnPointSelected.Position, null, Color.Red, 0f, new Vector2(_spawnTexture.Width / 2, _spawnTexture.Height / 2), 1f, (SpawnPointsPanel.CurrentSpawnPointSelected.Side == Side.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f);
                        }
                        else
                        {
                            if (SpawnPointsPanel.CurrentSpawnPointSelected != null)
                            {
                                spriteBatch.Draw(_spawnTexture, SpawnPointsPanel.CurrentSpawnPointSelected.Position, null, Color.White, 0f, new Vector2(_spawnTexture.Width / 2, _spawnTexture.Height / 2), 1f, (SpawnPointsPanel.CurrentSpawnPointSelected.Side == Side.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f);
                            }
                        }
                    }

                    foreach (Entity entity in Entities)
                    {
                        if (_lightGizmoToggled)
                        {
                            ColorEmitter colorEmitter = entity.GetComponent<ColorEmitter>();

                            if (colorEmitter != null && colorEmitter.Debug)
                            {
                                float scale = colorEmitter.Range / _colorEmitterCircleTexture.Width * 2;
                                spriteBatch.Draw(_colorEmitterCircleTexture, entity.transform.Position - new Vector2(_colorEmitterCircleTexture.Width * scale / 2, _colorEmitterCircleTexture.Height * scale / 2), null, colorEmitter.currentColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                                spriteBatch.Draw(_colorEmitterTexture, entity.transform.Position - new Vector2(_colorEmitterTexture.Width / 2, _colorEmitterTexture.Height / 2), colorEmitter.Color);
                            }
                        }

                        if (_boundsGizmoToggled)
                        {
                            CameraBound cameraBound = entity.GetComponent<CameraBound>();

                            if (cameraBound != null)
                            {
                                float angle = 0.0f;

                                switch (cameraBound.BoundSide)
                                {
                                    case Side.Left:
                                        angle = 0.0f;
                                        break;

                                    case Side.Down:
                                        angle = (float)(Math.PI + Math.PI / 2);
                                        break;

                                    case Side.Right:
                                        angle = (float)Math.PI;
                                        break;

                                    case Side.Up:
                                        angle = (float)(Math.PI / 2);
                                        break;
                                }

                                spriteBatch.Draw(_boundTexture, entity.transform.Position, null, Color.White, angle, new Vector2(_boundTexture.Width / 2, _boundTexture.Height / 2), 1f, SpriteEffects.None, 0);
                            }
                        }                      
                    }
                }
            }
            base.ManualDrawGame(spriteBatch);
        }

        public override void ManualDrawHUD(SpriteBatch sb)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.get_transformation(Neon.GraphicsDevice));
            if (CurrentTool != null)
                CurrentTool.Draw(sb);
            sb.End();
            sb.Begin();
            base.ManualDrawHUD(sb);
        }

        public void RefreshInspector(Entity SelectedEntity)
        {
            if (SelectedEntity != null)
            {
                this.SelectedEntity = SelectedEntity;

                RightDockControl.InspectorControl.InstantiateProperties(SelectedEntity);
            }
            else
            {
                RightDockControl.InspectorControl.ClearInspector();
            }
        }

        public void ManageInspector()
        {
            if (SelectedEntity != null)
            {
                ManagingInspector = true;
                foreach (PropertyComponentControl pcc in RightDockControl.InspectorControl.PropertyControlList)
                {
                    if (pcc.ctrl.Name == "X")
                    {
                        Vector2 v = (Vector2)pcc.pi.GetValue(pcc.c, null);
                        (pcc.ctrl as NumericUpDown).Value = (decimal)v.X;
                    }
                    else if(pcc.ctrl.Name == "Y")
                    {
                        Vector2 v = (Vector2)pcc.pi.GetValue(pcc.c, null);
                        (pcc.ctrl as NumericUpDown).Value = (decimal)v.Y;
                    }
                    else if (pcc.pi.PropertyType.IsEnum)
                    {
                        (pcc.ctrl as ComboBox).SelectedValue = (Enum)(pcc.pi.GetValue(pcc.c, null));
                    }
                    else if (pcc.pi.PropertyType == typeof(float))
                    {
                        float value = (float)pcc.pi.GetValue(pcc.c, null);
                        (pcc.ctrl as NumericUpDown).Value = (decimal)value;
                    }
                }

                if (Neon.Input.Pressed(Keys.Delete) && FocusedTextBox == null && FocusedNumericUpDown == null)
                {
                    //ActionManager.SaveAction(ActionType.DeleteEntity, new object[2] { DataManager.SavePrefab(SelectedEntity), this });
                    SelectedEntity.Destroy();
                    SelectedEntity = null;
                    RightDockControl.InspectorControl.ClearInspector();                 
                }
                ManagingInspector = false;
            }

            if (FocusedTextBox != null)
                ManageText();
            if (FocusedNumericUpDown != null)
                ManageNumber();
        }

        public void ManageText()
        {
            if (FocusedTextBox != null)
            {
                int LastSelectionStart = FocusedTextBox.SelectionStart;

                 for (int i = 0; i < Neon.Input.KeysPressed.Length; i++)
                     if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]) && Neon.Input.KeysPressed[i].ToString().Split('d')[0] == "NumPa")
                     {
                         int SelectionStart = FocusedTextBox.SelectionStart;

                         if(FocusedTextBox.SelectedText.Length > 0)
                         {
                             FocusedTextBox.Text = FocusedTextBox.Text.Replace(FocusedTextBox.SelectedText, "");
                         }
                              
                         FocusedTextBox.Text = FocusedTextBox.Text.Insert(SelectionStart,
                              Neon.Input.KeysPressed[i].ToString().Split('d')[1]);
                         FocusedTextBox.SelectionStart = ++LastSelectionStart;
                     }
                     else if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]) && Neon.Input.KeysPressed[i].ToString().Length == 1)
                     {
                         int SelectionStart = FocusedTextBox.SelectionStart;
                         if (FocusedTextBox.SelectedText.Length > 0)
                         {
                             FocusedTextBox.Text = FocusedTextBox.Text.Replace(FocusedTextBox.SelectedText, "");
                         }
                         FocusedTextBox.Text = FocusedTextBox.Text.Insert(SelectionStart,
                             Neon.Input.Check(Keys.LeftShift) || Neon.Input.Check(Keys.RightShift) ? Neon.Input.KeysPressed[i].ToString().ToUpper() : Neon.Input.KeysPressed[i].ToString().ToLower());
                         FocusedTextBox.SelectionStart = ++LastSelectionStart;
                         
                     }
                     else
                     {
                         if (Neon.Input.KeysPressed[i] == Keys.OemQuestion && PressedDelay <= 0.0f)
                         {
                             PressedDelay = 0.2f;
                             int SelectionStart = FocusedTextBox.SelectionStart;
                             if (FocusedTextBox.SelectedText.Length > 0)
                             {
                                 string[] text = FocusedTextBox.Text.Split(FocusedTextBox.SelectedText.ToCharArray());
                                 FocusedTextBox.Text = "";
                                 foreach (string s in text)
                                 {
                                     FocusedTextBox.Text += s;
                                 }
                             }
                             FocusedTextBox.Text = FocusedTextBox.Text.Insert(SelectionStart,
                                 ":");
                             FocusedTextBox.SelectionStart = ++LastSelectionStart;
                         }
                         else if (Neon.Input.KeysPressed[i] == Keys.D4 && PressedDelay <= 0.0f)
                         {
                             PressedDelay = 0.2f;
                             int SelectionStart = FocusedTextBox.SelectionStart;
                             if (FocusedTextBox.SelectedText.Length > 0)
                             {
                                 string[] text = FocusedTextBox.Text.Split(FocusedTextBox.SelectedText.ToCharArray());
                                 FocusedTextBox.Text = "";
                                 foreach (string s in text)
                                 {
                                     FocusedTextBox.Text += s;
                                 }
                             }
                             FocusedTextBox.Text = FocusedTextBox.Text.Insert(SelectionStart,
                                 "'");
                             FocusedTextBox.SelectionStart = ++LastSelectionStart;
                         }
                         else if (Neon.Input.KeysPressed[i] == Keys.Enter)
                         {
                             if (FocusedTextBox.Parent != null)
                                 FocusedTextBox.Parent.Focus();
                             else
                                 GameAsForm.Focus();
                             FocusedTextBox = null;
                         }
                         else if (Neon.Input.KeysPressed[i] == Keys.Delete && PressedDelay <= 0f)
                         {
                             if (FocusedTextBox.SelectionStart < FocusedTextBox.Text.Length)
                             {
                                 PressedDelay = 0.1f;
                                 if (FocusedTextBox.SelectionLength > 0)
                                 {
                                     FocusedTextBox.Text = FocusedTextBox.Text.Remove(FocusedTextBox.SelectionStart, FocusedTextBox.SelectionLength);
                                     FocusedTextBox.SelectionStart = LastSelectionStart;
                                 }
                                 else
                                 {
                                     FocusedTextBox.Text = FocusedTextBox.Text.Remove(FocusedTextBox.SelectionStart, 1);
                                     FocusedTextBox.SelectionStart = LastSelectionStart;
                                 }
                             }
                         }
                     }
            }
        }

        public void ManageNumber()
        {
            if (FocusedNumericUpDown != null)
            {
                for (int i = 0; i < Neon.Input.KeysPressed.Length; i++)
                {
                    if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]) && !(Neon.Input.Check(Keys.LeftShift) || Neon.Input.Check(Keys.RightShift)))
                    {
                        if (FocusedNumericUpDown != null)
                        {
                            int LastSelectionStart = (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart;
                            switch (Neon.Input.KeysPressed[i])
                            {
                                case Keys.D0:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "0");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D1:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "1");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D2:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "2");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D3:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "3");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D4:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "4");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D5:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "5");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D6:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "6");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D7:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "7");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D8:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "8");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.D9:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "9");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad0:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "0");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad1:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "1");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad2:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "2");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad3:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "3");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad4:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "4");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad5:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "5");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad6:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "6");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad7:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "7");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad8:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "8");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.NumPad9:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "9");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.OemPeriod:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        ".");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.OemComma:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        ".");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.Decimal:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        ".");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.Subtract:
                                    FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                        "-");
                                    (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                                    break;

                                case Keys.Enter:
                                    if (FocusedNumericUpDown.Parent != null)
                                        FocusedNumericUpDown.Parent.Focus();
                                    break;

                                case Keys.Delete:
                                    if (PressedDelay <= 0F)
                                    {

                                    }
                                    break;
                            }  
                        }
                       

                                             
                    }
                    else if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]))
                    {
                        if (FocusedNumericUpDown != null)
                        {
                            int LastSelectionStart = (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart;
                            if (Neon.Input.KeysPressed[i] == Keys.D6)
                            {
                                FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                    "-");
                                (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            }
                        }
                        
                    }
                }
            }
        }

        public override void AddEntity(Entity newEntity)
        {
            base.AddEntity(newEntity);
            
            if (BottomDockControl != null)
            {
                if (EditorVisible)
                {
                    BottomDockControl.entityListControl.InitializeEntityList();
                    BottomDockControl.entityListControl.SelectEntityNode(newEntity);
                }
            }      
        }

        public override void RemoveEntity(Entity entity)
        {
            base.RemoveEntity(entity);
            if (BottomDockControl != null)
            {
                if (EditorVisible)
                {
                    BottomDockControl.entityListControl.InitializeEntityList();
                    BottomDockControl.entityListControl.EntityListBox.SelectedNode = null;
                }
            }     
        }

        public override void ReloadLevel()
        {
            LeftDockControl.Hide();
            LeftDockControl.Dispose();
            BottomDockControl.Hide();
            BottomDockControl.Dispose();
            RightDockControl.Hide();
            RightDockControl.Dispose();
            if (AttacksSettingsManager != null)
                AttacksSettingsManager.Dispose();
            if (PathNodePanel != null)
                PathNodePanel.Dispose();
            if (SpawnPointsPanel != null)
                SpawnPointsPanel.Dispose();
            if (ElementSettingsManager != null)
                ElementSettingsManager.Dispose();
            if (AddComponentPanel != null)
                AddComponentPanel.Dispose();
            if (GraphicPicker != null)
                GraphicPicker.Dispose();
            if (SpritesheetPicker != null)
                SpritesheetPicker.Dispose();
            ChangeScreen(new EditorScreen(this.LevelMap.Group, this.LevelMap.Name, lastSpawnPointIndex, _statusToLoad, game, graphics));
        }

        public override void ChangeScreen(World nextScreen)
        {
            LeftDockControl.Hide();
            LeftDockControl.Dispose();
            BottomDockControl.Hide();
            BottomDockControl.Dispose();
            RightDockControl.Hide();
            RightDockControl.Dispose();
            if (AttacksSettingsManager != null)
                AttacksSettingsManager.Dispose();
            if (PathNodePanel != null)
                PathNodePanel.Dispose();
            if (SpawnPointsPanel != null)
                SpawnPointsPanel.Dispose();
            if (ElementSettingsManager != null)
                ElementSettingsManager.Dispose();
            if (AddComponentPanel != null)
                AddComponentPanel.Dispose();
            if (GraphicPicker != null)
                GraphicPicker.Dispose();
            if (SpritesheetPicker != null)
                SpritesheetPicker.Dispose();
            base.ChangeScreen(nextScreen);
        }

        public override void ChangeLevel(string groupName, string levelName, int spawnPointIndex)
        {
            LeftDockControl.Hide();
            LeftDockControl.Dispose();
            BottomDockControl.Hide();
            BottomDockControl.Dispose();
            RightDockControl.Hide();
            RightDockControl.Dispose();
            if (AttacksSettingsManager != null)
                AttacksSettingsManager.Dispose();
            if (PathNodePanel != null)
                PathNodePanel.Dispose();
            if (SpawnPointsPanel != null)
                SpawnPointsPanel.Dispose();
            if (ElementSettingsManager != null)
                ElementSettingsManager.Dispose();
            if (AddComponentPanel != null)
                AddComponentPanel.Dispose();
            if (GraphicPicker != null)
                GraphicPicker.Dispose();
            if (SpritesheetPicker != null)
                SpritesheetPicker.Dispose();
            ChangeScreen(new LoadingScreen(Neon.Game, spawnPointIndex, groupName, levelName, SaveStatus()));
        }

        public override void ChangeLevel(XElement savedStatus)
        {           
            ChangeScreen(new LoadingScreen(Neon.Game, savedStatus));
        }
    }
}
