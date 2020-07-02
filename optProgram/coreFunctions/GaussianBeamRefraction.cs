using optProgram.elements;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optProgram.coreFunctions
{
    public class GaussianBeamRefraction
    {
        public Beam GaussianCalculate(Sphere sphere, Beam incidentBeam,RefractiveIndex incidentIndex, RefractiveIndex exitIndex)
        {
            /*这里肯定错！！如果更改sphere的i和ip必须用指针！！*/
            sphere.CalculateIncidentAngle(incidentBeam, incidentIndex, exitIndex);
            double a, b;
            a = sphere.i + incidentBeam.u - sphere.ip;
            b = sphere.r + sphere.r * sphere.ip / a;
            Beam exitBeam = new Beam(a, b);

            return exitBeam;
        }
    }
}