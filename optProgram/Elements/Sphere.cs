using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace optProgram.elements
{
    public class Sphere
    {
        public double r,i,iprime;
        public Sphere(double r)
        {
            this.r = r;
        }
        public void CalculateIncidentAngle(Beam incidentBeam,double incidentIndex,double exitIndex)
        {
            this.i = (incidentBeam.l- this.r) * incidentBeam.u / this.r;
            this.iprime = this.i * incidentIndex / exitIndex;
        }
        
    }
}
