using NeonEngine;
using NeonEngine.AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NeonStarEditor
{
    public partial class StateCreation : Form
    {
        BindingList<State> StateList = new BindingList<State>();
        BindingList<AI> AIList = new BindingList<AI>();
        BindingList<State> AIStateList = new BindingList<State>();
        
        Type[] ConditionsActionsTypeList;

        State CurrentState;

        int CurrentLastID = 0;

        BindingList<AICondition> ConditionsBindingList = new BindingList<AICondition>();
        BindingList<AIAction> ActionsBindingList = new BindingList<AIAction>();

        public StateCreation()
        {
            InitializeComponent();

            LoadStateFile();

            this.ConditionsList.DrawMode = DrawMode.OwnerDrawFixed;
            this.AIStates.DrawMode = DrawMode.OwnerDrawFixed;
            this.ConditionsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(ConditionsList_DrawItem);
            this.ActionsList.DrawMode = DrawMode.OwnerDrawFixed;
            this.ActionsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(ConditionsList_DrawItem);
            StateList.AllowEdit = true;
            this.SelectedState.DataSource = StateList;
            this.SelectedState.DisplayMember = "TagName";

            this.SelectedAI.DataSource = AIList;
            this.SelectedAI.DisplayMember = "TagName";

            ConditionsActionsTypeList = Neon.utils.GetTypesInNamespace(typeof(Neon).Assembly, "NeonEngine.AI");
            foreach (Type t in ConditionsActionsTypeList)
                if (t.BaseType.Equals(typeof(AICondition)))
                    this.ConditionsAvailable.Items.Add(t);
                else if (t.BaseType.Equals(typeof(AIAction)))
                    this.ActionsAvailable.Items.Add(t);

            this.ConditionsAvailable.DisplayMember = "Name";
            this.ConditionsAvailable.SelectedIndex = 0;
            this.ActionsAvailable.DisplayMember = "Name";
            this.ActionsAvailable.SelectedIndex = 0;

            this.SelectedAI.SelectedIndexChanged += SelectedAI_SelectedIndexChanged;
        }

        #region Load & Save
        private void LoadStateFile()
        {
            Stream stream = File.OpenRead(@"Data\AIStates.xml");

            XDocument document = XDocument.Load(stream);

            XElement XnaContent = document.Element("XnaContent");
            XElement States = XnaContent.Element("States");

            int LastID = 0;

            foreach (XElement s in States.Elements("State"))
            {
                LastID = int.Parse(s.Attribute("ID").Value);
                State state = new State(LastID);

                state.TagName = s.Attribute("Name").Value;

                foreach (XElement a in s.Elements("Action"))
                {
                    AIAction action = (AIAction)Activator.CreateInstance(Type.GetType(a.Attribute("Type").Value + ", " + a.Attribute("Type").Value.Split('.')[0]));
                    state.Actions.Add(action);
                    foreach (XElement setting in a.Elements("Setting"))
                    {
                        Type type = action.GetType();
                        PropertyInfo pi = type.GetProperty(setting.Attribute("Name").Value);
                        if (pi.PropertyType.IsEnum)
                            pi.SetValue(action, Enum.Parse(pi.PropertyType, setting.Value), null);
                        else
                            pi.SetValue(action, Convert.ChangeType(setting.Value, pi.PropertyType), null);
                    }
                }
                foreach (XElement c in s.Elements("Condition"))
                {
                    AICondition condition = (AICondition)Activator.CreateInstance(Type.GetType(c.Attribute("Type").Value + ", " + c.Attribute("Type").Value.Split('.')[0]));
                    state.Conditions.Add(condition);
                    foreach (XElement setting in c.Elements("Setting"))
                    {
                        Type type = condition.GetType();
                        
                        PropertyInfo pi = type.GetProperty(setting.Attribute("Name").Value);
                        if (pi.PropertyType.IsEnum)
                            pi.SetValue(condition, Enum.Parse(pi.PropertyType, setting.Value), null);
                        else
                            pi.SetValue(condition, Convert.ChangeType(setting.Value, pi.PropertyType), null);
                    }
                }
                StateList.Add(state);
            }

            CurrentLastID = LastID;

            stream.Close();

            stream = File.OpenRead(@"Data\AIPatterns.xml");

            document = XDocument.Load(stream);

            XnaContent = document.Element("XnaContent");
            XElement AIs = XnaContent.Element("Patterns");

            foreach (XElement p in AIs.Elements("Pattern"))
            {
                LastID = int.Parse(p.Attribute("ID").Value);
                AI ai = new AI(LastID);

                ai.TagName = p.Attribute("Name").Value;

                foreach (XElement s in p.Element("States").Elements("State"))
                {
                    State state = new State(int.Parse(s.Attribute("ID").Value));
                    
                    state.TagName = s.Attribute("Name").Value;

                    foreach (XElement a in s.Elements("Action"))
                    {
                        AIAction action = (AIAction)Activator.CreateInstance(Type.GetType(a.Attribute("Type").Value + ", " + a.Attribute("Type").Value.Split('.')[0]));
                        state.Actions.Add(action);
                        foreach (XElement setting in a.Elements("Setting"))
                        {
                            Type type = action.GetType();
                            PropertyInfo pi = type.GetProperty(setting.Attribute("Name").Value);
                            if (pi.PropertyType.IsEnum)
                                pi.SetValue(action, Enum.Parse(pi.PropertyType, setting.Value), null);
                            else
                                pi.SetValue(action, Convert.ChangeType(setting.Value, pi.PropertyType), null);
                        }
                    }
                    foreach (XElement c in s.Elements("Condition"))
                    {
                        AICondition condition = (AICondition)Activator.CreateInstance(Type.GetType(c.Attribute("Type").Value + ", " + c.Attribute("Type").Value.Split('.')[0]));
                        condition.ConditionID = int.Parse(c.Attribute("ID").Value);
                        state.Conditions.Add(condition);
                        foreach (XElement setting in c.Elements("Setting"))
                        {
                            Type type = condition.GetType();
                            PropertyInfo pi = type.GetProperty(setting.Attribute("Name").Value);
                            if (pi.PropertyType.IsEnum)
                                pi.SetValue(condition, Enum.Parse(pi.PropertyType, setting.Value), null);
                            else
                                pi.SetValue(condition, Convert.ChangeType(setting.Value, pi.PropertyType), null);
                        }
                    }
                    ai.StatesList.Add(state);
                }

                foreach(XElement cl in p.Element("ConditionLinks").Elements("Link"))
                    ai.StateSwitch.Add(ai.GetConditionByID(int.Parse(cl.Element("Condition").Attribute("ID").Value)), ai.GetStateByID(int.Parse(cl.Element("State").Attribute("ID").Value)));

                AIList.Add(ai);
            }

            stream.Close();
        }

        private void SaveAI_Click(object sender, EventArgs e)
        {
            Stream stream = File.Create(@"Data\AIPatterns.xml");

            XDocument document = new XDocument();

            XElement XnaContent = new XElement("XnaContent");
            XElement Patterns = new XElement("Patterns");

            foreach (AI ai in AIList)
            {
                XElement Pattern = new XElement("Pattern", new XAttribute("ID", ai.GetAIID), new XAttribute("Name", ai.TagName));
                XElement States = new XElement("States");

                foreach (State s in ai.StatesList)
                {
                    XElement State = new XElement("State", new XAttribute("ID", s.GetStateID), new XAttribute("Name", s.TagName));
                    foreach (AICondition c in s.Conditions)
                    {
                        XElement Condition = new XElement("Condition", new XAttribute("Type", c.GetType().ToString()), new XAttribute("ID", c.ConditionID));
                        PropertyInfo[] properties = c.GetType().GetProperties();
                        foreach (PropertyInfo pi in properties)
                        {
                            if (pi.Name != "TagName")
                            {
                                XElement Setting = new XElement("Setting", new XAttribute("Name", pi.Name));
                                Setting.Value = pi.GetValue(c, null).ToString();
                                Condition.Add(Setting);
                            }
                        }
                        State.Add(Condition);
                    }
                    foreach (AIAction aa in s.Actions)
                    {
                        XElement Action = new XElement("Action", new XAttribute("Type", aa.GetType().ToString()));
                        PropertyInfo[] properties = aa.GetType().GetProperties();
                        foreach (PropertyInfo pi in properties)
                        {
                            if (pi.Name != "TagName")
                            {
                                XElement Setting = new XElement("Setting", new XAttribute("Name", pi.Name));
                                Setting.Value = pi.GetValue(aa, null).ToString();
                                Action.Add(Setting);
                            }
                        }
                        State.Add(Action);
                    }
                    States.Add(State);
                }
                Pattern.Add(States);

                XElement ConditionLinks = new XElement("ConditionLinks");

                foreach (KeyValuePair<AICondition, State> kvp in ai.StateSwitch)
                {
                    XElement Link = new XElement("Link");
                    XElement Condition = new XElement("Condition", new XAttribute("ID", kvp.Key.ConditionID));
                    XElement State = new XElement("State", new XAttribute("ID", kvp.Value.GetStateID));

                    Link.Add(Condition, State);

                    ConditionLinks.Add(Link);
                }
                Pattern.Add(ConditionLinks);

                Patterns.Add(Pattern);
            }
            XnaContent.Add(Patterns);
            document.Add(XnaContent);
            document.Save(stream);
            stream.Close();
        }

        private void SaveState_Click(object sender, EventArgs e)
        {
           Stream stream = File.Create(@"Data\AIStates.xml");

            XDocument document = new XDocument();

            XElement XnaContent = new XElement("XnaContent");
            XElement States = new XElement("States");

            foreach (State s in StateList)
            {
                XElement State = new XElement("State", new XAttribute("ID", s.GetStateID), new XAttribute("Name", s.TagName != null ? s.TagName : "Untitled"));
                foreach (AICondition ac in s.Conditions)
                {
                    XElement Condition = new XElement("Condition", new XAttribute("Type", ac.GetType().ToString()));
                    PropertyInfo[] properties = ac.GetType().GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (pi.Name != "TagName")
                        {
                            XElement Setting = new XElement("Setting", new XAttribute("Name", pi.Name));
                            Setting.Value = pi.GetValue(ac, null).ToString();
                            Condition.Add(Setting);
                        }
                    }
                    State.Add(Condition);
                }

                foreach (AIAction aa in s.Actions)
                {
                    XElement Action = new XElement("Action", new XAttribute("Type", aa.GetType().ToString()));
                    PropertyInfo[] properties = aa.GetType().GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (pi.Name != "TagName")
                        {
                            XElement Setting = new XElement("Setting", new XAttribute("Name", pi.Name));
                            Setting.Value = pi.GetValue(aa, null).ToString();
                            Action.Add(Setting);
                        }
                    }
                    State.Add(Action);
                }

                States.Add(State);
            }

            XnaContent.Add(States);
            document.Add(XnaContent);
            document.Save(stream);
            stream.Close();
        }
        #endregion

        #region Cosmetic
        private void ConditionsList_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < (sender as ListBox).Items.Count)
            {
                string text = ((sender as ListBox).Items[index] is AICondition ? ((sender as ListBox).Items[index] as AICondition).TagName : ((sender as ListBox).Items[index] as AIAction).TagName);
                Graphics g = e.Graphics;

                Color color = (selected) ? Color.Gray : Color.FromArgb(0, 0, 0, 0);
                g.FillRectangle(new SolidBrush(color), e.Bounds);

                g.DrawString(text, e.Font, new SolidBrush(Color.FromArgb(240, 240, 240)),
                    (sender as ListBox).GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }

        private void AIStates_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < (sender as ListBox).Items.Count)
            {
                string text = ((sender as ListBox).Items[index] as State).TagName;
                Graphics g = e.Graphics;

                Color color = (selected) ? Color.Gray : Color.FromArgb(0, 0, 0, 0);
                g.FillRectangle(new SolidBrush(color), e.Bounds);

                g.DrawString(text, e.Font, new SolidBrush(Color.FromArgb(240, 240, 240)),
                    (sender as ListBox).GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }
        #endregion    

        #region DataGridView Management
        private void AddColumns(Type type, bool Actions, bool readOnly)
        {
            PropertyInfo[] properties = type.GetProperties();

            DataGridViewCell Cell;
            foreach (PropertyInfo pi in properties.Where(pi => pi.Name != "TagName"))
            {
                Type valueType = pi.PropertyType;
                if (valueType.IsEnum)
                {
                    Cell = new DataGridViewComboBoxCell();

                    ((DataGridViewComboBoxCell)Cell).DataSource
                        = Enum.GetValues(valueType);
                }
                else if (valueType.Equals(typeof(Boolean)))
                {
                    Cell = new DataGridViewCheckBoxCell();
                }
                else
                {
                    Cell = new DataGridViewTextBoxCell();
                }

                Cell.ValueType = valueType;
                Cell.Value = 0;

                if (!Actions)
                {
                    this.CurrentConditionSettings.Rows.Add();
                    this.CurrentConditionSettings.Rows[this.CurrentConditionSettings.Rows.Count - 1].Cells["Setting"].Value = pi;
                    this.CurrentConditionSettings.Rows[this.CurrentConditionSettings.Rows.Count - 1].Cells["Value"] = Cell;
                    this.CurrentConditionSettings.Rows[this.CurrentConditionSettings.Rows.Count - 1].Cells["Value"].Value = pi.GetValue(ConditionsList.SelectedItem, null);
                }
                else
                {
                    this.CurrentActionSettings.Rows.Add();
                    this.CurrentActionSettings.Rows[this.CurrentActionSettings.Rows.Count - 1].Cells["ActionSetting"].Value = pi;
                    this.CurrentActionSettings.Rows[this.CurrentActionSettings.Rows.Count - 1].Cells["ActionValue"] = Cell;
                    this.CurrentActionSettings.Rows[this.CurrentActionSettings.Rows.Count - 1].Cells["ActionValue"].Value = pi.GetValue(ActionsList.SelectedItem, null);
                }

                Cell.ReadOnly = false;
            }

        }

        private void CurrentConditionSettings_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void ConditionsLinksData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            AICondition condition = (AICondition)this.ConditionsLinksData.Rows[e.RowIndex].Cells["Condition"].Value;
            if(SelectedAI.SelectedItem != null)
                (SelectedAI.SelectedItem as AI).StateSwitch[condition] = (State)(ConditionsLinksData.Rows[e.RowIndex].Cells["LinkedTo"] as DataGridViewComboBoxCell).Value;
        }

        private void CurrentConditionSettings_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            PropertyInfo pi = (PropertyInfo)CurrentConditionSettings.Rows[e.RowIndex].Cells["Setting"].Value;
            pi.SetValue(ConditionsList.SelectedItem, CurrentConditionSettings.Rows[e.RowIndex].Cells["Value"].Value, null);
        }

        private void CurrentActionSettings_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            PropertyInfo pi = (PropertyInfo)CurrentActionSettings.Rows[e.RowIndex].Cells["ActionSetting"].Value;
            pi.SetValue(ActionsList.SelectedItem, CurrentActionSettings.Rows[e.RowIndex].Cells["ActionValue"].Value, null);
        }
        #endregion

        #region Actions & Conditions
        private void AddAction_Click(object sender, EventArgs e)
        {
            if (SelectedState.SelectedItem != null)
                this.ActionsBindingList.Add((AIAction)Activator.CreateInstance((Type)this.ActionsAvailable.SelectedItem));
        }

        private void ActionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActionsList.SelectedItem != null)
            {
                CurrentActionSettings.Rows.Clear();
                AddColumns(this.ActionsList.SelectedItem.GetType(), true, true);
                if (this.CurrentActionSettings.SelectedRows.Count > 0)
                    this.CurrentActionSettings.SelectedRows[0].Selected = false;
            }
        }       
        private void AddCondition_Click(object sender, EventArgs e)
        {
            this.ConditionsBindingList.Add((AICondition)Activator.CreateInstance((Type)this.ConditionsAvailable.SelectedItem));
        }

        private void ConditionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConditionsList.SelectedItem != null)
            {
                CurrentConditionSettings.Rows.Clear();
                AddColumns(this.ConditionsList.SelectedItem.GetType(), false, true);
                if (this.CurrentConditionSettings.SelectedRows.Count > 0)
                    this.CurrentConditionSettings.SelectedRows[0].Selected = false;
            }
        }
        #endregion

        #region State & AI Management
        private void StateName_TextChanged(object sender, EventArgs e)
        {
            if ((this.SelectedState.SelectedItem as State) != null && CurrentState == (this.SelectedState.SelectedItem as State))
            {
                (this.SelectedState.SelectedItem as State).TagName = (sender as TextBox).Text;
                StateList.ResetItem(this.SelectedState.SelectedIndex);
            }
            else if((AIStates.SelectedItem as State) != null)
            {
                CurrentState.TagName = (sender as TextBox).Text;
                AIStateList.ResetItem(this.AIStates.SelectedIndex);
            }
            else
                (sender as TextBox).Text = "";
        }

        private void CreateState_Click(object sender, EventArgs e)
        {
            CurrentLastID++;
            State state = new State(CurrentLastID);
            state.TagName = "Untitled";
            StateList.Add(state);
            SelectedState.SelectedIndex = StateList.Count - 1;
        }

        private void SelectedState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedState.SelectedItem != null)
            {
                CurrentState = this.SelectedState.SelectedItem as State;
                ChangingState();
            }
        }

        private void ChangingState()
        {
            this.StateName.Text = CurrentState.TagName;

            this.ConditionsBindingList = new BindingList<AICondition>(CurrentState.Conditions);
            this.ConditionsList.DisplayMember = "TagName";
            this.ConditionsList.DataSource = this.ConditionsBindingList;
            CurrentConditionSettings.Rows.Clear();

            this.ActionsBindingList = new BindingList<AIAction>(CurrentState.Actions);
            this.ActionsList.DisplayMember = "TagName";
            this.ActionsList.DataSource = this.ActionsBindingList;
            CurrentActionSettings.Rows.Clear();
        }

        private void SelectedAI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedAI.SelectedItem != null)
            {
                this.AIName.Text = (this.SelectedAI.SelectedItem as AI).TagName;

                this.AIStateList = new BindingList<State>((this.SelectedAI.SelectedItem as AI).StatesList);
                this.AIStates.DisplayMember = "TagName";
                this.AIStates.DataSource = AIStateList;

                this.ConditionsLinksData.Rows.Clear();
            }
        }

        private void AIName_TextChanged(object sender, EventArgs e)
        {
            if ((this.SelectedAI.SelectedItem as AI) != null)
            {
                (this.SelectedAI.SelectedItem as AI).TagName = (sender as TextBox).Text;
                AIList.ResetItem(this.SelectedAI.SelectedIndex);
            }
            else
                (sender as TextBox).Text = "";
        }

        private void AIStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AIStates.SelectedItem != null)
            {
                CurrentState = AIStates.SelectedItem as State;
                ChangingState();

                ConditionsLinksData.Rows.Clear();

                if (AIStates.SelectedItem != null)
                    foreach (AICondition ac in (AIStates.SelectedItem as State).Conditions)
                    {
                        this.ConditionsLinksData.Rows.Add();
                        this.ConditionsLinksData.Rows[this.ConditionsLinksData.Rows.Count - 1].Cells["Condition"].Value = ac;
                        (this.ConditionsLinksData.Rows[this.ConditionsLinksData.Rows.Count - 1].Cells["LinkedTo"] as DataGridViewComboBoxCell).DataSource = new BindingList<State>((SelectedAI.SelectedItem as AI).StatesList);
                        (this.ConditionsLinksData.Rows[this.ConditionsLinksData.Rows.Count - 1].Cells["LinkedTo"] as DataGridViewComboBoxCell).DisplayMember = "TagName";
                        (this.ConditionsLinksData.Rows[this.ConditionsLinksData.Rows.Count - 1].Cells["LinkedTo"] as DataGridViewComboBoxCell).ValueMember = "ThisState";
                        if (!(SelectedAI.SelectedItem as AI).StateSwitch.ContainsKey(ac))
                            (SelectedAI.SelectedItem as AI).StateSwitch.Add(ac, AIStates.SelectedItem as State);
                        
                        this.ConditionsLinksData.Rows[this.ConditionsLinksData.Rows.Count - 1].Cells["LinkedTo"].Value = (SelectedAI.SelectedItem as AI).StateSwitch[ac];
                    }


                if (this.ConditionsLinksData.SelectedRows.Count > 0)
                    this.ConditionsLinksData.SelectedRows[0].Selected = false;
            }
        }
        #endregion

        private void AddToAI_Click(object sender, EventArgs e)
        {
            if (SelectedAI.SelectedItem != null)
            {
                State state = new State(CurrentLastID);
                state.TagName = CurrentState.TagName;
                foreach (AIAction a in CurrentState.Actions)
                {
                    AIAction newA = (AIAction)Activator.CreateInstance(a.GetType(), null);
                    Type type = a.GetType();
                    foreach (PropertyInfo pi in type.GetProperties())
                    {
                        pi.SetValue(newA, pi.GetValue(a, null), null);
                    }
                    state.Actions.Add(newA);
                }
                int id = 0;
                while ((SelectedAI.SelectedItem as AI).GetConditionByID(id) != null)
                    id++;
                foreach (AICondition c in CurrentState.Conditions)
                {
                    AICondition newC = (AICondition)Activator.CreateInstance(c.GetType(), null);
                    newC.ConditionID = id;
                    id++;
                    Type type = c.GetType();
                    foreach (PropertyInfo pi in type.GetProperties())
                    {
                        pi.SetValue(newC, pi.GetValue(c, null), null);
                    }
                    state.Conditions.Add(newC);
                    (SelectedAI.SelectedItem as AI).StateSwitch.Add(newC, state);
                }
                AIStateList.Add(state);
                
            }
        }

        private void NewAI_Click(object sender, EventArgs e)
        {
            AI ai = new AI(AIList[AIList.Count - 1].GetAIID + 1);
            ai.TagName = "Untitled";
            AIList.Add(ai);
            SelectedAI.SelectedIndex = SelectedAI.Items.Count - 1;
        }


    }
}
