using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary
{
    static public class ElementManager
    {
        static public List<List<object>> ThunderParameters = new List<List<object>>();
        //static public List<List<object>> FireParameters = new List<List<object>>();

        static public void LoadElementParameters()
        {
            XElement parameters = XDocument.Load(@"../Data/Config/Elements.xml").Element("Elements");
            LoadThunderParameters(parameters);
        }

        static private void LoadThunderParameters(XElement parameters)
        {
            XElement thunderParameters = parameters.Element("Thunder");

            List<object> firstLevelParameters = new List<object>();

            firstLevelParameters.Add(float.Parse(thunderParameters.Element("FirstLevel").Element("GaugeCost").Value.ToString(), CultureInfo.InvariantCulture));
            firstLevelParameters.Add(float.Parse(thunderParameters.Element("FirstLevel").Element("DashHorizontalImpulse").Value.ToString(), CultureInfo.InvariantCulture));
            firstLevelParameters.Add(float.Parse(thunderParameters.Element("FirstLevel").Element("DashDuration").Value.ToString(), CultureInfo.InvariantCulture));
            firstLevelParameters.Add(thunderParameters.Element("FirstLevel").Element("AttackToLaunch").Value.ToString());

            ThunderParameters.Add(firstLevelParameters);

            List<object> secondLevelParameters = new List<object>();

            secondLevelParameters.Add(float.Parse(thunderParameters.Element("SecondLevel").Element("GaugeCost").Value.ToString(), CultureInfo.InvariantCulture));
            secondLevelParameters.Add(float.Parse(thunderParameters.Element("SecondLevel").Element("DashHorizontalImpulse").Value.ToString(), CultureInfo.InvariantCulture));
            secondLevelParameters.Add(float.Parse(thunderParameters.Element("SecondLevel").Element("DashDuration").Value.ToString(), CultureInfo.InvariantCulture));
            secondLevelParameters.Add(thunderParameters.Element("SecondLevel").Element("AttackToLaunch").Value.ToString());
            secondLevelParameters.Add(float.Parse(thunderParameters.Element("SecondLevel").Element("DashVerticalUpImpulse").Value.ToString(), CultureInfo.InvariantCulture));
            secondLevelParameters.Add(float.Parse(thunderParameters.Element("SecondLevel").Element("DashVerticalDownImpulse").Value.ToString(), CultureInfo.InvariantCulture));

            ThunderParameters.Add(secondLevelParameters);

            List<object> thirdLevelParameters = new List<object>();

            thirdLevelParameters.Add(float.Parse(thunderParameters.Element("ThirdLevel").Element("GaugeCost").Value.ToString(), CultureInfo.InvariantCulture));
            thirdLevelParameters.Add(float.Parse(thunderParameters.Element("ThirdLevel").Element("DashHorizontalImpulse").Value.ToString(), CultureInfo.InvariantCulture));            
            thirdLevelParameters.Add(float.Parse(thunderParameters.Element("ThirdLevel").Element("DashDuration").Value.ToString(), CultureInfo.InvariantCulture));
            thirdLevelParameters.Add(thunderParameters.Element("ThirdLevel").Element("AttackToLaunch").Value.ToString());
            thirdLevelParameters.Add(float.Parse(thunderParameters.Element("ThirdLevel").Element("DashVerticalUpImpulse").Value.ToString(), CultureInfo.InvariantCulture));
            thirdLevelParameters.Add(float.Parse(thunderParameters.Element("ThirdLevel").Element("DashVerticalDownImpulse").Value.ToString(), CultureInfo.InvariantCulture));
            thirdLevelParameters.Add(float.Parse(thunderParameters.Element("ThirdLevel").Element("StunDuration").Value.ToString(), CultureInfo.InvariantCulture));

            ThunderParameters.Add(thirdLevelParameters);

            List<object> assimilationParameters = new List<object>();

            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("ModifierDuration").Value.ToString()));
            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("MovementSpeedModifier").Value.ToString()));
            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("AttackSpeedModifier").Value.ToString()));

            ThunderParameters.Add(assimilationParameters);
        }
    }
}
