using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optProgram.elements
{
    public class Sphere
    {
        public double r,i,ip;
        public Sphere(double r)
        {
            this.r = r;
        }
        public void CalculateIncidentAngle(Beam incidentBeam,RefractiveIndex incidentIndex,RefractiveIndex exitIndex)
        {
            this.i = (incidentBeam.l - this.r) * incidentBeam.u / this.r;
            this.ip = this.i * incidentIndex.value / exitIndex.value;

        }
        
    }
}
