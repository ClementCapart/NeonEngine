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
        static public List<List<object>> FireParameters = new List<List<object>>();

        static public void LoadElementParameters()
        {
            XElement parameters = XDocument.Load(@"../Data/Config/Elements.xml").Element("Elements");
            LoadFireParameters(parameters);
            LoadThunderParameters(parameters);
        }

        static private void LoadFireParameters(XElement parameters)
        {
            XElement fireParameters = parameters.Element("Fire");

            List<object> generalParameters = new List<object>();
            generalParameters.Add(float.Parse(fireParameters.Element("General").Element("GaugeSpeed").Value));
            generalParameters.Add(float.Parse(fireParameters.Element("General").Element("MaxChargeDuration").Value));

            FireParameters.Add(generalParameters);

            List<object> firstLevelParameters = new List<object>();
            firstLevelParameters.Add(float.Parse(fireParameters.Element("FirstLevel").Element("GaugeCost").Value));
            firstLevelParameters.Add(fireParameters.Element("FirstLevel").Element("StageOneAttack").Value);
            firstLevelParameters.Add(fireParameters.Element("FirstLevel").Element("StageTwoAttack").Value);
            firstLevelParameters.Add(fireParameters.Element("FirstLevel").Element("StageThreeAttack").Value);
            firstLevelParameters.Add(float.Parse(fireParameters.Element("FirstLevel").Element("StageTwoThreshold").Value));
            firstLevelParameters.Add(float.Parse(fireParameters.Element("FirstLevel").Element("StageThreeThreshold").Value));

            FireParameters.Add(firstLevelParameters);

            List<object> secondLevelParameters = new List<object>();
            secondLevelParameters.Add(float.Parse(fireParameters.Element("SecondLevel").Element("GaugeCost").Value));
            secondLevelParameters.Add(fireParameters.Element("SecondLevel").Element("StageOneAttack").Value);
            secondLevelParameters.Add(fireParameters.Element("SecondLevel").Element("StageTwoAttack").Value);
            secondLevelParameters.Add(fireParameters.Element("SecondLevel").Element("StageThreeAttack").Value);
            secondLevelParameters.Add(float.Parse(fireParameters.Element("SecondLevel").Element("StageTwoThreshold").Value));
            secondLevelParameters.Add(float.Parse(fireParameters.Element("SecondLevel").Element("StageThreeThreshold").Value));
            secondLevelParameters.Add(fireParameters.Element("SecondLevel").Element("StageFourAttack").Value);
            secondLevelParameters.Add(float.Parse(fireParameters.Element("SecondLevel").Element("StageFourThreshold").Value));

            FireParameters.Add(secondLevelParameters);

            List<object> thirdLevelParameters = new List<object>();
            thirdLevelParameters.Add(float.Parse(fireParameters.Element("ThirdLevel").Element("GaugeCost").Value));
            thirdLevelParameters.Add(fireParameters.Element("ThirdLevel").Element("StageOneAttack").Value);
            thirdLevelParameters.Add(fireParameters.Element("ThirdLevel").Element("StageTwoAttack").Value);
            thirdLevelParameters.Add(fireParameters.Element("ThirdLevel").Element("StageThreeAttack").Value);
            thirdLevelParameters.Add(float.Parse(fireParameters.Element("ThirdLevel").Element("StageTwoThreshold").Value));
            thirdLevelParameters.Add(float.Parse(fireParameters.Element("ThirdLevel").Element("StageThreeThreshold").Value));
            thirdLevelParameters.Add(fireParameters.Element("ThirdLevel").Element("StageFourAttack").Value);
            thirdLevelParameters.Add(float.Parse(fireParameters.Element("ThirdLevel").Element("StageFourThreshold").Value));

            FireParameters.Add(thirdLevelParameters);

            List<object> assimilationParameters = new List<object>();
            assimilationParameters.Add(float.Parse(fireParameters.Element("Assimilation").Element("DamageModifier").Value));
            assimilationParameters.Add(float.Parse(fireParameters.Element("Assimilation").Element("ModifierDurationLevelOne").Value));
            assimilationParameters.Add(float.Parse(fireParameters.Element("Assimilation").Element("ModifierDurationLevelTwo").Value));
            assimilationParameters.Add(float.Parse(fireParameters.Element("Assimilation").Element("ModifierDurationLevelThree").Value));

            FireParameters.Add(assimilationParameters);
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

            ThunderParameters.Add(thirdLevelParameters);

            List<object> assimilationParameters = new List<object>();

            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("FirstLevel").Element("ModifierDuration").Value.ToString()));
            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("FirstLevel").Element("MovementSpeedModifier").Value.ToString()));
            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("FirstLevel").Element("AttackSpeedModifier").Value.ToString()));

            ThunderParameters.Add(assimilationParameters);

            assimilationParameters = new List<object>();

            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("SecondLevel").Element("ModifierDuration").Value.ToString()));
            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("SecondLevel").Element("MovementSpeedModifier").Value.ToString()));
            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("SecondLevel").Element("AttackSpeedModifier").Value.ToString()));

            ThunderParameters.Add(assimilationParameters);

            assimilationParameters = new List<object>();

            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("ThirdLevel").Element("ModifierDuration").Value.ToString()));
            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("ThirdLevel").Element("MovementSpeedModifier").Value.ToString()));
            assimilationParameters.Add(float.Parse(thunderParameters.Element("Assimilation").Element("ThirdLevel").Element("AttackSpeedModifier").Value.ToString()));

            ThunderParameters.Add(assimilationParameters);
        }
    }
}
