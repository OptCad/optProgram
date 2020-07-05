using optProgram.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace optProgram.elements
{
    public class OptSystem
    {
        Queue<double> RefractiveIndexOn = new Queue<double>();
        Queue<double> RadiusOn = new Queue<double>();
        Queue<double> IntervalOn = new Queue<double>();
        Queue<double> RefractiveIndexOff = new Queue<double>();
        Queue<double> RadiusOff = new Queue<double>();
        Queue<double> IntervalOff = new Queue<double>();
        Queue<double> IncidentAngleQ = new Queue<double>();
        Queue<double> ExitAngleQ = new Queue<double>();
        double pupilDiameter;
        Obj obj;
        bool isInfinite;


        Dictionary<string, Beam> outputRealOn = new Dictionary<string, Beam> { };
        Dictionary<string, Beam> outputRealOff = new Dictionary<string, Beam> { };
        Dictionary<string, Beam> outputGaussianOff = new Dictionary<string, Beam> { };
        Beam outputGaussianOn;

        Dictionary<string, double> basicPoints = new Dictionary<string, double> { };
        Dictionary<string, double> idealHeight = new Dictionary<string, double> { };


        public OptSystem(Queue<Sphere> systemdata, Obj obj, bool isInfinite)
        {
            this.isInfinite = isInfinite;
            this.obj = obj;
            this.pupilDiameter = obj.pupilDiameter;
            this.RefractiveIndexOn.Enqueue(obj.envRefractive);
            int inputCount = systemdata.Count;
            while (systemdata.Count > 0)
            {

                Sphere tmp = systemdata.Dequeue();
                RadiusOn.Enqueue(tmp.r);
                RefractiveIndexOn.Enqueue(tmp.n);

                if (inputCount != 1 && systemdata.Count != 0)
                    IntervalOn.Enqueue(tmp.d);
            }
        }

        public void calAll()
        {
            double[] K1 = new double[] { 1, 0.85, 0.7, 0.5, 0.3 };
            double[] K2 = new double[] { 1, 0.85, 0.7, 0.5, 0.3, 0, -1, -0.85, -0.7, -0.5, -0.3 };
            Beam incidentBeamGaussianOn;
            Dictionary<string, Beam> incidentBeamGaussianOff;
            Dictionary<string, Beam> incidentBeamRealOn;
            Dictionary<string, Beam> incidentBeamRealOff;

            RadiusOff = new Queue<double>(RadiusOn);
            IntervalOff = new Queue<double>(IntervalOn);
            RefractiveIndexOff = new Queue<double>(RefractiveIndexOn);

            basicPoints = calBasicPoints(outputGaussianOn);
            incidentBeamGaussianOff = initOffaxialGaussian(K1);
            incidentBeamRealOff = initOffAxialReal(K1, K2);

            incidentBeamGaussianOn = initOnaxialGaussian();
            incidentBeamRealOn = initOnAxialReal(K2);



            outputGaussianOn = GaussianRefraction(incidentBeamGaussianOn, new Queue<double>(RadiusOn), new Queue<double>(RefractiveIndexOn), new Queue<double>(IntervalOn));

            MessageBox.Show("l:" + outputGaussianOn.l.ToString() + "\nu:" + outputGaussianOn.u.ToString());

            foreach (KeyValuePair<string, Beam> kvp in incidentBeamRealOn)
            {
                outputRealOn.Add(kvp.Key, RealRefraction(kvp.Value, new Queue<double>(RadiusOn), new Queue<double>(RefractiveIndexOn), new Queue<double>(IntervalOn)));
            }
            foreach (KeyValuePair<string, Beam> kvp in incidentBeamGaussianOff)
            {
                outputGaussianOff.Add(kvp.Key, GaussianRefraction(kvp.Value, new Queue<double>(RadiusOff), new Queue<double>(RefractiveIndexOff), new Queue<double>(IntervalOff)));
            }
            /*Beam outputReal = RealRefraction(incidentBeam, isInfinite);
            MessageBox.Show("l:" + outputReal.l.ToString() + "\nu:" + outputReal.u.ToString());*/
            //sphericalAber(outputRealOn, outputGaussian);
            idealHeight=idealH(outputGaussianOff);

        }

        private Beam GaussianRefraction(Beam incidentBeam1, Queue<double> Radius,
            Queue<double> RefractiveIndex, Queue<double> Interval) //Calculate exit beam using recursion
        {

            if (Radius.Count == 0)
                return incidentBeam1;
            double u1p, l1p, u2, l2;
            double incidentAngle, exitAngle;
            double radius = Radius.Dequeue();
            double n = RefractiveIndex.Dequeue();
            double np = RefractiveIndex.Peek(); //Do not delete np item for later use: n2 = n1p
            incidentAngle = (incidentBeam1.l - radius) * incidentBeam1.u / radius;
            exitAngle = n * incidentAngle / np;
            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);
            u1p = incidentAngle + incidentBeam1.u - exitAngle;
            l1p = radius + radius * exitAngle / u1p;

            Beam exitBeam1 = new Beam(l1p, u1p);
            if (Radius.Count == 0) return exitBeam1; //the last sphere does not have any sphere behind

            u2 = u1p;
            l2 = l1p - Interval.Dequeue();
            Beam incidentBeam2 = new Beam(l2, u2);

            return GaussianRefraction(incidentBeam2, Radius, RefractiveIndex, Interval);
        }


        private Beam RealRefraction(Beam incidentBeam1, Queue<double> Radius,
            Queue<double> RefractiveIndex, Queue<double> Interval) //Calculate exit beam using recursion
        {

            double u1p, l1p, u2, l2;
            double incidentAngle, exitAngle;
            double radius = Radius.Dequeue();
            double n = RefractiveIndex.Dequeue();
            double np = RefractiveIndex.Peek(); //Do not delete np item for later use: n2 = n1p

            double sinI;
            sinI = (incidentBeam1.l - radius) * Math.Sin(incidentBeam1.u) / radius;
            if (Math.Abs(sinI) > 1)
            {
                MessageBox.Show("入射光线超半球！");
            }
            incidentAngle = Math.Asin(sinI);
            double sinIp = n * sinI / np;
            if (Math.Abs(sinIp) > 1)
            {
                MessageBox.Show("发生全反射！");
            }
            exitAngle = Math.Asin(sinIp);

            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);

            u1p = incidentAngle + incidentBeam1.u - exitAngle;
            l1p = radius + radius * sinIp / Math.Sin(u1p);

            Beam exitBeam1 = new Beam(l1p, u1p);
            if (Radius.Count == 0) return exitBeam1; // the last sphere does not have any sphere behind

            u2 = u1p;
            l2 = l1p - Interval.Dequeue();
            Beam incidentBeam2 = new Beam(l2, u2);

            return RealRefraction(incidentBeam2, Radius, RefractiveIndex, Interval);
        }

        private Beam initOnaxialGaussian()
        {
            Beam beam;
            Queue<double> RefractiveIndex = new Queue<double>(RefractiveIndexOn);
            Queue<double> Radius = new Queue<double>(RadiusOn);
            Queue<double> Interval = new Queue<double>(IntervalOn);
            if (isInfinite == true)
            {

                double n_tmp = RefractiveIndex.Dequeue();
                double np_tmp = RefractiveIndex.Peek();
                double r_tmp = Radius.Dequeue();
                double fp = np_tmp / (np_tmp - n_tmp) * r_tmp;
                double lp_tmp;
                if (Interval.Count != 0)
                    lp_tmp = fp - Interval.Dequeue();
                else
                    lp_tmp = fp;
                beam = new Beam(lp_tmp, pupilDiameter / 2 / fp);

            }
            else
                beam = new Beam(obj.objDistance, Math.Sin(obj.apertureAngle));

            return beam;
        }
        private Dictionary<string, Beam> initOffaxialGaussian(double[] K1s)
        {
            Dictionary<string, Beam> beam = new Dictionary<string, Beam>();

            foreach (double K1 in K1s)
            {
                if (isInfinite == true)
                    beam.Add(K1.ToString(), new Beam(0, Math.Sin(K1 * obj.fieldAngle)));
                else
                    beam.Add("2", new Beam(0, Math.Sin(Math.Atan(obj.objHeight / obj.objDistance))));

            }

            return beam;
        }

        private Dictionary<string, Beam> initOnAxialReal(double[] K2s)
        {
            Dictionary<string, Beam> beam = new Dictionary<string, Beam> { };
            Queue<double> RefractiveIndex = new Queue<double>(RefractiveIndexOn);
            Queue<double> Radius = new Queue<double>(RadiusOn);
            Queue<double> Interval = new Queue<double>(IntervalOn);
            int flag = 0;
            foreach (double K2 in K2s)
            {
                RefractiveIndex = new Queue<double>(RefractiveIndexOn);
                Radius = new Queue<double>(RadiusOn);
                Interval = new Queue<double>(IntervalOn);
                if (isInfinite == true)
                {
                    flag = 1;
                    double n_tmp = RefractiveIndex.Dequeue();
                    double np_tmp = RefractiveIndex.Peek();
                    double r_tmp = Radius.Dequeue();
                    double i = Math.Asin(K2 * pupilDiameter / 2 / r_tmp);
                    double lp_tmp;
                    double up = i - Math.Asin(n_tmp / np_tmp * Math.Sin(i));
                    if (Interval.Count != 0)
                        lp_tmp = r_tmp + r_tmp * n_tmp / np_tmp * Math.Sin(i) / Math.Sin(up) - Interval.Dequeue();
                    else
                        lp_tmp = r_tmp + r_tmp * n_tmp / np_tmp * Math.Sin(i) / Math.Sin(up);

                    beam.Add(K2.ToString(), new Beam(lp_tmp, up));
                }
                else
                    beam.Add(K2.ToString(), new Beam(obj.objDistance, Math.Asin(K2 * Math.Sin(obj.apertureAngle))));
            }
            if (flag == 1)
            {
                if (IntervalOn.Count != 0)
                    IntervalOn.Dequeue();
                RadiusOn.Dequeue();
                RefractiveIndexOn.Dequeue();
            }
            return beam;
        }

        private Dictionary<string, Beam> initOffAxialReal(double[] K1s, double[] K2s)
        {
            Dictionary<string, Beam> beam = new Dictionary<string, Beam> { };
            foreach (double K1 in K1s)
            {
                foreach (double K2 in K2s)
                {
                    if (isInfinite == true)
                        beam.Add(K1.ToString() + "  " + K2.ToString(), new Beam(K2 * pupilDiameter / 2 / Math.Tan(K1 * obj.fieldAngle), K1 * obj.fieldAngle));
                    else
                    {
                        double tanU1;
                        tanU1 = (K1 * obj.objHeight - K2 * pupilDiameter / 2) / obj.objDistance;
                        beam.Add(K1.ToString() + "  " + K2.ToString(), new Beam(K2 * pupilDiameter / 2 / tanU1, Math.Atan(tanU1)));
                    }
                }
            }

            return beam;
        }

        private void sphericalAber(Dictionary<string, Beam> outputReal, Beam outputGaussian)
        {
            Dictionary<string, double> SphericalAber = new Dictionary<string, double>();

            Beam outputR1 = outputReal["1"];
            Beam outputR2 = outputReal["0.7"];
            SphericalAber.Add("1", outputR1.l - outputGaussian.l);
            SphericalAber.Add("0.7", outputR2.l - outputGaussian.l);
            MessageBox.Show("全孔径球差：" + SphericalAber["1"].ToString() + "\n 0.7孔径球差:" + SphericalAber["0.7"]);

        }

        private Dictionary<string, double> idealH(Dictionary<string, Beam> outputGaussian)
        {
            Dictionary<string, double> tmp = new Dictionary<string, double>();
            double beta = -(outputGaussianOn.l - basicPoints["lp"] - basicPoints["fp"]) / basicPoints["fp"];
            double idealH;
            foreach (KeyValuePair<string, Beam> kvp in outputGaussian)

            {
                if (isInfinite)

                {
                    idealH = Math.Tan(outputGaussian[kvp.Key].u) * (outputGaussianOn.l - outputGaussian[kvp.Key].l);
                    tmp.Add(kvp.Key, idealH);
                }
                else
                {
                    idealH = beta * obj.objHeight;
                    tmp.Add(kvp.Key, idealH);
                }
            }
            

            return tmp;
        }

        private Dictionary<string, double> calBasicPoints(Beam outputGaussian)
        {
            Beam incident_tmp;
            Queue<double> RefractiveIndex = new Queue<double>(RefractiveIndexOn);
            Queue<double> Radius = new Queue<double>(RadiusOn);
            Queue<double> Interval = new Queue<double>(IntervalOn);
            Dictionary<string, double> basicP = new Dictionary<string, double>();

            incident_tmp = new Beam(0, 0.0001);
            Beam output_tmp = GaussianRefraction(incident_tmp, new Queue<double>(Radius), new Queue<double>(RefractiveIndex), new Queue<double>(Interval));
            double lp = output_tmp.l;
            basicP.Add("lp", lp);
            double n_tmp = RefractiveIndex.Dequeue();
            double np_tmp = RefractiveIndex.Peek();
            double r_tmp = Radius.Dequeue();
            double fp_tmp = np_tmp / (np_tmp - n_tmp) * r_tmp;
            double lp_tmp;
            if (Interval.Count != 0)
                lp_tmp = fp_tmp - Interval.Dequeue();
            else
                lp_tmp = fp_tmp;
            incident_tmp = new Beam(lp_tmp, 0.0001 / fp_tmp);
            output_tmp = GaussianRefraction(incident_tmp, new Queue<double>(Radius), new Queue<double>(RefractiveIndex), new Queue<double>(Interval));
            double fp = 0.0001 / Math.Tan(output_tmp.u);
            basicP.Add("fp", fp);
            double lH = output_tmp.l - fp;
            basicP.Add("lH", lH);
            MessageBox.Show(lp.ToString());
            return basicP;
        }
    }
}
