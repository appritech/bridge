using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BridgeIface
{
    public enum FloatIntBoolNone
    {
        Float,
        Int,
        Bool,
        None
    }

    public class DHControl
    {
        Control m_control;
        FloatIntBoolNone m_type;
        string m_dhName;
        bool m_isOutput;
        float m_scaler;

        public DHControl(string dhName, FloatIntBoolNone type, Control LinkedControl, bool isOutput, float scaler = 1.0f)
        {
            m_dhName = dhName;
            m_type = type;
            m_control = LinkedControl;
            m_isOutput = isOutput;
            m_scaler = scaler;
            if (m_scaler == 0.0f)
                m_scaler = 1.0f;

            if (m_isOutput)
            {
                if (m_control is CheckBox)
                {
                    ((CheckBox)m_control).Click += new EventHandler(checkBoxEventHandler);
                }

                if (m_control is TextBox)
                {
                    ((TextBox)m_control).KeyDown += new KeyEventHandler(textBoxEventHandler);
                }

                if (m_control is VScrollBar)
                {
                    //((VScrollBar)m_control).Scroll += new ScrollEventHandler(scrollEventHandler);
                    ((VScrollBar)m_control).ValueChanged += new EventHandler(DHControl_ValueChanged);
                }

                if (m_control is HScrollBar)
                {
                    //((HScrollBar)m_control).Scroll += new ScrollEventHandler(scrollEventHandler);
                    ((HScrollBar)m_control).ValueChanged += new EventHandler(DHControl_ValueChanged);
                }

                if (m_control is TrackBar)
                {
                    ((TrackBar)m_control).ValueChanged += new EventHandler(DHControl_ValueChanged);
                }
            }
        }

        void DHControl_ValueChanged(object sender, EventArgs e)
        {
            writeToDataHolder();
        }

        private void checkBoxEventHandler(object sender, EventArgs e)
        {
            writeToDataHolder();
        }

        private void textBoxEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                writeToDataHolder();
        }

        private void scrollEventHandler(object sender, ScrollEventArgs e)
        {
            writeToDataHolder();
        }


        public void readFromDataHolder()
        {
            try
            {
                if (m_control is CheckBox)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        ((CheckBox)m_control).Checked = Convert.ToBoolean(DataHolderIface.GetIntVal(m_dhName));
                    else if (m_type == FloatIntBoolNone.Float)
                        ((CheckBox)m_control).Checked = Convert.ToBoolean(DataHolderIface.GetFloatVal(m_dhName) / m_scaler);
                }

                if (m_control is TextBox)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        ((TextBox)m_control).Text = Convert.ToString(DataHolderIface.GetIntVal(m_dhName));
                    else if (m_type == FloatIntBoolNone.Float)
                        ((TextBox)m_control).Text = Convert.ToString(DataHolderIface.GetFloatVal(m_dhName) / m_scaler);
                }

                if (m_control is VScrollBar)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        ((VScrollBar)m_control).Value = Convert.ToInt32(DataHolderIface.GetIntVal(m_dhName));
                    else if (m_type == FloatIntBoolNone.Float)
                        ((VScrollBar)m_control).Value = Convert.ToInt32(DataHolderIface.GetFloatVal(m_dhName) / m_scaler);
                }

                if (m_control is HScrollBar)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        ((HScrollBar)m_control).Value = Convert.ToInt32(DataHolderIface.GetIntVal(m_dhName));
                    else if (m_type == FloatIntBoolNone.Float)
                        ((HScrollBar)m_control).Value = Convert.ToInt32(DataHolderIface.GetFloatVal(m_dhName) / m_scaler);
                }

                if (m_control is TrackBar)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        ((TrackBar)m_control).Value = Convert.ToInt32(DataHolderIface.GetIntVal(m_dhName));
                    else if (m_type == FloatIntBoolNone.Float)
                        ((TrackBar)m_control).Value = Convert.ToInt32(DataHolderIface.GetFloatVal(m_dhName) / m_scaler);
                }
            }
            catch (Exception)
            {
            }
        }

        public void writeToDataHolder()
        {
            try
            {
                if (!m_isOutput)
                    return;

                if (m_control is CheckBox)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        DataHolderIface.SetIntVal(m_dhName, Convert.ToInt32(((CheckBox)m_control).Checked));
                    else if (m_type == FloatIntBoolNone.Float)
                        DataHolderIface.SetFloatVal(m_dhName, Convert.ToSingle(((CheckBox)m_control).Checked) * m_scaler);
                }

                if (m_control is TextBox)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        DataHolderIface.SetIntVal(m_dhName, Convert.ToInt32(((TextBox)m_control).Text));
                    else if (m_type == FloatIntBoolNone.Float)
                        DataHolderIface.SetFloatVal(m_dhName, Convert.ToSingle(((TextBox)m_control).Text) * m_scaler);
                }

                if (m_control is VScrollBar)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        DataHolderIface.SetIntVal(m_dhName, Convert.ToInt32(((VScrollBar)m_control).Value));
                    else if (m_type == FloatIntBoolNone.Float)
                        DataHolderIface.SetFloatVal(m_dhName, Convert.ToSingle(((VScrollBar)m_control).Value) * m_scaler);
                }

                if (m_control is HScrollBar)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        DataHolderIface.SetIntVal(m_dhName, Convert.ToInt32(((HScrollBar)m_control).Value));
                    else if (m_type == FloatIntBoolNone.Float)
                        DataHolderIface.SetFloatVal(m_dhName, Convert.ToSingle(((HScrollBar)m_control).Value) * m_scaler);
                }

                if (m_control is TrackBar)
                {
                    if (m_type == FloatIntBoolNone.Int)
                        DataHolderIface.SetIntVal(m_dhName, Convert.ToInt32(((TrackBar)m_control).Value));
                    else if (m_type == FloatIntBoolNone.Float)
                        DataHolderIface.SetFloatVal(m_dhName, Convert.ToSingle(((TrackBar)m_control).Value) * m_scaler);
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
