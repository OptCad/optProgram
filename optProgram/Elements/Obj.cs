using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optProgram.elements
{
    public class Obj
    {
        public double objDistance, apertureAngle, envRefractive, objHeight, fieldAngle, pupilDiameter;


        public Obj(double distance = 0, double angle = 0, double env_n = 0,
            double objHeight = 0, double fieldAng = 0, double pupilD = 0)
        {
            this.apertureAngle = angle;
            this.objDistance = distance;
            this.envRefractive = env_n;
            this.objHeight = objHeight;
            this.fieldAngle = fieldAng;
            this.pupilDiameter = pupilD;
        }
    }
}
