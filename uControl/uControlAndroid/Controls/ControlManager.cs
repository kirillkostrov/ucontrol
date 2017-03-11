using System;

namespace uControlAndroid.Controls
{
    public class ControlManager
    {
        public void SaveControl(Control control)
        {
            if(!control.Id.HasValue)
            {
                control.Id = Guid.NewGuid();
            }
        }

        public Control[] GetAllControls()
        {
            return new Control[0];
        }

        public Control GetAllControls(Guid id)
        {
            return new Control();
        }
    }
}