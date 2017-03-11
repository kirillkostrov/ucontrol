
namespace uControlAndroid
{
    class BootMouseReport
    {
        bool button1;
        bool button2;
        bool button3;
        int xDisplacement;
        int yDisplacement;

        public byte[] getRawValue()
        {
            byte[] value = new byte[4];
            int button = 0;
            if (this.button1)
            {
                button = 0 | 1;
            }
            if (this.button2)
            {
                button |= 2;
            }
            if (this.button3)
            {
                button |= 4;
            }
            value[0] = (byte)button;
            value[1] = (byte)this.xDisplacement;
            value[2] = (byte)this.yDisplacement;
            return value;
        }

        public bool isButton1()
        {
            return this.button1;
        }

        public void setButton1(bool button1)
        {
            this.button1 = button1;
        }

        public bool isButton2()
        {
            return this.button2;
        }

        public void setButton2(bool button2)
        {
            this.button2 = button2;
        }

        public bool isButton3()
        {
            return this.button3;
        }

        public void setButton3(bool button3)
        {
            this.button3 = button3;
        }

        public int getXDisplacement()
        {
            return this.xDisplacement;
        }

        public void setXDisplacement(int xDisplacement)
        {
            this.xDisplacement = xDisplacement;
        }

        public int getYDisplacement()
        {
            return this.yDisplacement;
        }

        public void setYDisplacement(int yDisplacement)
        {
            this.yDisplacement = yDisplacement;
        }
    }
}