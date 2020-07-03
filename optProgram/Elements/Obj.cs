using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optProgram.elements
{
    public class Obj
    {
        public double objDistance, apertureAngle,envRefractive;

        public Obj(double distance,double angle,double env_n)
        {
            this.apertureAngle = angle;
            this.objDistance = distance;
            this.envRefractive = env_n;
        }
    }
}
