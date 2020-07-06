using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optProgram.elements
{
    public class Beam
    {
        public double u, l;
        public Beam( double l,double u)
        {
            this.u = u;
            this.l = l;
        }
    }

    public class astigBeam
    {
        public double u, l,s,t,x;
        public astigBeam(double l, double u,double s,double t,double x)
        {
            this.u = u;
            this.l = l;
            this.s = s;
            this.t = t;
            this.x = x;
        }
    }
}
