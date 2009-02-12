using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Karts.Code
{
    class ControllerManager
    {
        private Dictionary<String, Control>[] controls;

        private static ControllerManager controllerManager;

        private ControllerManager(){
            controls = new Dictionary<string, Control>[4];
            for (int i = 0; i < controls.Length; ++i)
                controls[i] = new Dictionary<string, Control>();
        }

        public static ControllerManager GetInstance()
        {
            if(controllerManager == null)
                controllerManager = new ControllerManager();
            return controllerManager;
        }

        public void parse(){
            String dir = Directory.GetCurrentDirectory() + "\\Content\\controls.xml";
            XmlTextReader reader = new XmlTextReader(dir);
            reader.Read();

            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            XmlElement controlsNode = (XmlElement) doc.GetElementsByTagName("controls")[0];

#if WINDOWS
            XmlElement elem = (XmlElement) controlsNode.GetElementsByTagName("pc")[0];
#else
            XmlElement elem = controlsNode.GetElementById("x360");
#endif

            XmlNodeList nodes = elem.GetElementsByTagName("player");
		    for (int i = 0; i < nodes.Count; ++i) {
                XmlNode node = nodes[i];
                int playerIndex = Convert.ToInt16(node.Attributes["id"].InnerXml);

                XmlNodeList controlNodes = ((XmlElement)node).GetElementsByTagName("axis");
                for (int j = 0; j < controlNodes.Count; ++j)
                {
                    XmlNode controlNode = controlNodes[j];
                    String name = controlNode.Attributes["name"].InnerXml;
                    int index = Convert.ToInt16(controlNode.Attributes["controller"].InnerXml);
                    bool left = controlNode.Attributes["thumb"].InnerXml.ToString().ToLower().Equals("left");
                    bool x = controlNode.Attributes["axis"].InnerXml.ToString().ToLower().Equals("x");
                    Control control = new PadAxisControl(index, left, x);
                    controls[playerIndex].Add(name, control);
                }

                controlNodes = ((XmlElement)node).GetElementsByTagName("button");
                for (int j = 0; j < controlNodes.Count; ++j)
                {
                    XmlNode controlNode = controlNodes[j];
                    String name = controlNode.Attributes["name"].InnerXml;
                    int index = Convert.ToInt16(controlNode.Attributes["controller"].InnerXml);
                    String buttonStr = controlNode.Attributes["button"].InnerXml;
                    Buttons button = (Buttons) Enum.Parse(typeof(Buttons), buttonStr);  
                    Control control = new PadButtonControl(button, index);
                    controls[playerIndex].Add(name, control);
                }

                controlNodes = ((XmlElement)node).GetElementsByTagName("key");
                for (int j = 0; j < controlNodes.Count; ++j)
                {
                    XmlNode controlNode = controlNodes[j];
                    String name = controlNode.Attributes["name"].InnerXml;
                    String keyStr = controlNode.Attributes["key"].InnerXml;
                    Keys key = (Keys)Enum.Parse(typeof(Keys), keyStr);
                    Control control = new KeyboardControl(key);
                    controls[playerIndex].Add(name, control);
                }
            }
        }

        public bool isPressed(int playerIndex, String control) { return controls[playerIndex][control].isPressed(); }
        public bool isDown(int playerIndex, String control) { return controls[playerIndex][control].isDown(); ; }
        public float getInputValue(int playerIndex, String control) { return controls[playerIndex][control].getInputValue(); ; }
    }

    class Control
    {
        protected enum CONTROLLER_TYPE
        {
            PAD_AXIS = 0,
            PAD_AXIS_BUTTON = 1,
            PAD_BUTTON = 2,
            KEYBOARD = 3,
            MOUSE = 4
        }

        protected CONTROLLER_TYPE controllerType;

        public virtual bool isPressed(){ return false;}
        public virtual bool isDown(){ return false;}
        public virtual float getInputValue() { return 0; }
    }

    class PadButtonControl : Control
    {
        private Buttons button;
        private int index;

        public PadButtonControl(Buttons button, int index)
        {
            controllerType = CONTROLLER_TYPE.PAD_BUTTON;
            this.button = button;
            this.index = index;
        }

        public override bool isPressed() { return InputManager.GetInstance().isButtonPressed(index, button); }
        public override bool isDown() { return InputManager.GetInstance().isButtonDown(index, button); }
        public override float getInputValue() { return isDown() ? 1 : 0; }
    }

    class PadAxisControl : Control
    {
        private int index;
        private bool left;
        private bool x;

        public PadAxisControl(int index, bool left, bool x)
        {
            controllerType = CONTROLLER_TYPE.PAD_AXIS;
            this.index = index;
            this.left = left;
            this.x = x;
        }

        public override float getInputValue() { return InputManager.GetInstance().getAxis(index, left, x); }
    }

    class KeyboardControl : Control
    {
        private Keys key;

        public KeyboardControl(Keys key)
        {
            controllerType = CONTROLLER_TYPE.KEYBOARD;
            this.key = key;
        }

        public override bool isPressed() { return InputManager.GetInstance().isKeyPressed(key); }
        public override bool isDown() { return InputManager.GetInstance().isKeyDown(key); }
        public override float getInputValue() { return isDown() ? 1 : 0; }
    }

    class MouseControl : Control
    {
        public MouseControl()
        {
            controllerType = CONTROLLER_TYPE.MOUSE;
        }
    }
}
