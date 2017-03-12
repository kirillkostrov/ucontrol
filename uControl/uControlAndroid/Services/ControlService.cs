using System;
using uControlAndroid.Entities;

namespace uControlAndroid.Services
{
    public class ControlService
    {
        public void SaveControl(Control control)
        {
            if(control.Id == 0)
            {
                control.Id = 0;
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