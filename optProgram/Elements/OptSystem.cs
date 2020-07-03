using optProgram.coreFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optProgram.elements
{
    public class OptSystem
    {
        Queue<double> RefractiveIndexQ = new Queue<double>();
        Queue<double> RadiusQ = new Queue<double>();
        Queue<double> IntervalQ = new Queue<double>();
        Queue<double> IncidentAngleQ = new Queue<double>();
        Queue<double> ExitAngleQ = new Queue<double>();
        public OptSystem(Queue<double> systemdata)
        {
            int Qlength = systemdata.Count ;
            int k = 1;
            while(systemdata.Count>0)
            {
                 
                if(k == 1)   //first item in data queue
                {
                    RefractiveIndexQ.Enqueue(systemdata.Dequeue());
                }else if(k == Qlength)    //last item in data queue
                {
                    RefractiveIndexQ.Enqueue(systemdata.Dequeue());
                }else
                {
                    if((k+1)%3 == 0)
                        RadiusQ.Enqueue(systemdata.Dequeue());
                    else if(k%3 == 0)
                        RefractiveIndexQ.Enqueue(systemdata.Dequeue());
                    else
                        IntervalQ.Enqueue(systemdata.Dequeue());

                }

                k++;
            }

        }
        public Beam GaussianRefraction(Beam incidentBeam1) //Calculate exit beam using recursion
        {
            
            double u1p, l1p,u2,l2;
            double incidentAngle, exitAngle;
            double radius = RadiusQ.Dequeue();
            double n = RefractiveIndexQ.Dequeue();
            double np = RefractiveIndexQ.Peek(); //Do not delete np item for later use: n2 = n1p
            
            incidentAngle = (incidentBeam1.l - radius) * incidentBeam1.u / radius;
            exitAngle = n*incidentAngle/np;
            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);

            u1p = incidentAngle + incidentBeam1.u - exitAngle;
            l1p = radius + radius * exitAngle / u1p;

            Beam exitBeam1 = new Beam(u1p, l1p);
            if (IntervalQ.Count == 0) return exitBeam1; // the last sphere does not have any sphere behind

            u2 = u1p;
            l2 = l1p - IntervalQ.Dequeue();
            Beam incidentBeam2 = new Beam(u2, l2);

            return GaussianRefraction(incidentBeam2);
        }
    }
}
