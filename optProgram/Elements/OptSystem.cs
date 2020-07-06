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
        astigBeam incident;
        Dictionary<string, Queue<double>> RIndexOn = new Dictionary<string, Queue<double>>();
        Queue<double> RadiusOn = new Queue<double>();
        Queue<double> IntervalOn = new Queue<double>();

        Dictionary<string, Queue<double>> RIndexOff = new Dictionary<string, Queue<double>>();
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
        Dictionary<string, Beam> outputGaussianOn = new Dictionary<string, Beam> { };

        Dictionary<string, double> basicPoints = new Dictionary<string, double> { };
        Dictionary<string, double> idealHeight = new Dictionary<string, double> { };
        Dictionary<string, double> realHeight = new Dictionary<string, double> { };


        public OptSystem(Queue<Sphere> systemdata, Obj obj, bool isInfinite)
        {
            Queue<double> RefractiveIndexOnd = new Queue<double>();
            Queue<double> RefractiveIndexOnF = new Queue<double>();
            Queue<double> RefractiveIndexOnC = new Queue<double>();
            this.isInfinite = isInfinite;
            this.obj = obj;
            this.pupilDiameter = obj.pupilDiameter;
            RefractiveIndexOnd.Enqueue(obj.envRefractive);
            RefractiveIndexOnF.Enqueue(obj.envRefractive);
            RefractiveIndexOnC.Enqueue(obj.envRefractive);
            int inputCount = systemdata.Count;
            while (systemdata.Count > 0)
            {

                Sphere tmp = systemdata.Dequeue();
                RadiusOn.Enqueue(tmp.r);
                RefractiveIndexOnd.Enqueue(tmp.nd);
                RefractiveIndexOnF.Enqueue(tmp.nF);
                RefractiveIndexOnC.Enqueue(tmp.nC);

                if (inputCount != 1 && systemdata.Count != 0)
                    IntervalOn.Enqueue(tmp.d);
            }
            RIndexOn.Add("d", RefractiveIndexOnd);
            RIndexOn.Add("F", RefractiveIndexOnF);
            RIndexOn.Add("C", RefractiveIndexOnC);
            RIndexOff.Add("d", new Queue<double>(RefractiveIndexOnd));
            RIndexOff.Add("F", new Queue<double>(RefractiveIndexOnF));
            RIndexOff.Add("C", new Queue<double>(RefractiveIndexOnC));

            //RIndexOff = new Dictionary<string, Queue<double>>(RIndexOn);
        }

        public void calAll()
        {
            double[] K1 = new double[] { 1, 0.85, 0.7, 0.5, 0.3 };
            double[] K2 = new double[] { 1, 0.85, 0.7, 0.5, 0.3, 0, -1, -0.85, -0.7, -0.5, -0.3 };

            Dictionary<string, Beam> incidentBeamGaussianOn;
            Dictionary<string, Beam> incidentBeamGaussianOff;
            Dictionary<string, Beam> incidentBeamRealOn;
            Dictionary<string, Beam> incidentBeamRealOff;

            if (isInfinite)
                incident = new astigBeam(0, obj.fieldAngle, Math.Pow(10, 15), Math.Pow(10, 15), 0);
            else
                incident = new astigBeam(obj.objDistance, Math.Atan(obj.objHeight/obj.objDistance), 
                    Math.Sqrt(obj.objHeight* obj.objHeight+obj.objDistance* obj.objDistance),
                    Math.Sqrt(obj.objHeight * obj.objHeight + obj.objDistance * obj.objDistance),
                    0);

            RadiusOff = new Queue<double>(RadiusOn);
            IntervalOff = new Queue<double>(IntervalOn);


            basicPoints = calBasicPoints();
            incidentBeamGaussianOff = initOffaxialGaussian(K1);
            incidentBeamRealOff = initOffAxialReal(K1, K2);

            incidentBeamGaussianOn = initOnaxialGaussian();
            incidentBeamRealOn = initOnAxialReal(K2);


            foreach (KeyValuePair<string, Queue<double>> kvp in RIndexOn)
            {
                outputGaussianOn.Add(kvp.Key, GaussianRefraction(incidentBeamGaussianOn[kvp.Key], new Queue<double>(RadiusOn),
                    new Queue<double>(kvp.Value), new Queue<double>(IntervalOn)));
            }

            //MessageBox.Show("l:" + outputGaussianOn.l.ToString() + "\nu:" + outputGaussianOn.u.ToString());
            foreach (KeyValuePair<string, Queue<double>> kvp1 in RIndexOff)
            {
                foreach (KeyValuePair<string, Beam> kvp2 in incidentBeamGaussianOff)
                {
                    outputGaussianOff.Add(kvp2.Key + "  " + kvp1.Key, GaussianRefraction(kvp2.Value, new Queue<double>(RadiusOff), new Queue<double>(kvp1.Value), new Queue<double>(IntervalOff)));
                }
            }
            foreach (KeyValuePair<string, Beam> kvp2 in incidentBeamRealOn)
            {
                string str = kvp2.Key;
                str = str.Substring(str.Length - 1, 1);
                outputRealOn.Add(kvp2.Key, RealRefraction(kvp2.Value, new Queue<double>(RadiusOn), new Queue<double>(RIndexOn[str]), new Queue<double>(IntervalOn)));
            }

            foreach (KeyValuePair<string, Queue<double>> kvp1 in RIndexOff)
            {
                foreach (KeyValuePair<string, Beam> kvp2 in incidentBeamRealOff)
                {
                    outputRealOff.Add(kvp2.Key + "  " + kvp1.Key, RealRefraction(kvp2.Value, new Queue<double>(RadiusOff), new Queue<double>(kvp1.Value), new Queue<double>(IntervalOff)));
                }
            }

            /*Beam outputReal = RealRefraction(incidentBeam, isInfinite);
            MessageBox.Show("l:" + outputReal.l.ToString() + "\nu:" + outputReal.u.ToString());*/
            //sphericalAber(outputRealOn, outputGaussian);
            idealHeight = idealH(outputGaussianOff);
            realHeight = realH(outputRealOff);
            astigBeam output = new astigBeam(0, 0, 0, 0, 0);
            output = ImageDiffRefraction(incident, new Queue<double>(RadiusOff), new Queue<double>(RIndexOff["d"]), new Queue<double>(IntervalOff));
            double ltp = output.t * Math.Cos(output.u) + output.x;
            double lsp = output.s * Math.Cos(output.u) + output.x;
            double xsp = lsp - output.l;
            MessageBox.Show(realHeight["0.7  0  d"].ToString() + "   " + realHeight["1  0  d"].ToString());
            //Distortion(idealHeight, realHeight);


        }

        private void Distortion(Dictionary<string, double> y0, Dictionary<string, double> yp)
        {
            double absoluteD1, relativeD1, absoluteD7, relativeD7;
            absoluteD1 = yp["1  0"] - y0["1"];
            relativeD1 = absoluteD1 / y0["1"] * 100;
            absoluteD7 = yp["0.7  0"] - y0["0.7"];
            relativeD7 = absoluteD7 / y0["0.7"] * 100;
            MessageBox.Show("绝对畸变  全视场：" + absoluteD1.ToString() + "0.7视场：" + absoluteD7.ToString() + "\n相对畸变  全视场：" + relativeD1.ToString() + "%0.7视场：" + relativeD7.ToString() + "%");
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

        private Dictionary<string, Beam> initOnaxialGaussian()
        {
            Dictionary<string, Beam> beam = new Dictionary<string, Beam>();
            Queue<double> RefractiveIndex = new Queue<double>();
            Queue<double> Radius = new Queue<double>(RadiusOn);
            Queue<double> Interval = new Queue<double>(IntervalOn);
            foreach (KeyValuePair<string, Queue<double>> kvp in RIndexOn)
            {
                RefractiveIndex = new Queue<double>(kvp.Value);
                Radius = new Queue<double>(RadiusOn);
                Interval = new Queue<double>(IntervalOn);
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
                    beam.Add(kvp.Key, new Beam(lp_tmp, pupilDiameter / 2 / fp));

                }
                else
                    beam.Add(kvp.Key, new Beam(obj.objDistance, Math.Sin(obj.apertureAngle)));
            }
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
                    beam.Add(K1.ToString(), new Beam(0, Math.Sin(Math.Atan(K1 * obj.objHeight / obj.objDistance))));

            }

            return beam;
        }

        private Dictionary<string, Beam> initOnAxialReal(double[] K2s)
        {
            Dictionary<string, Beam> beam = new Dictionary<string, Beam> { };
            Queue<double> RefractiveIndex = new Queue<double>();
            Queue<double> Radius = new Queue<double>(RadiusOn);
            Queue<double> Interval = new Queue<double>(IntervalOn);
            int flag = 0;
            foreach (KeyValuePair<string, Queue<double>> kvp in RIndexOn)
            {
                foreach (double K2 in K2s)
                {
                    RefractiveIndex = new Queue<double>(kvp.Value);
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

                        beam.Add(K2.ToString() + "  " + kvp.Key, new Beam(lp_tmp, up));
                    }
                    else
                        beam.Add(K2.ToString() + "  " + kvp.Key, new Beam(obj.objDistance, Math.Asin(K2 * Math.Sin(obj.apertureAngle))));
                }
            }
            if (flag == 1)
            {
                if (IntervalOn.Count != 0)
                    IntervalOn.Dequeue();
                RadiusOn.Dequeue();
                RIndexOn["d"].Dequeue();
                RIndexOn["F"].Dequeue();
                RIndexOn["C"].Dequeue();
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
                        beam.Add(K1.ToString() + "  " + K2.ToString(), new Beam(K2 * pupilDiameter / 2 / Math.Tan(K1 * obj.fieldAngle),
                            K1 * obj.fieldAngle));
                    else
                    {
                        double tanU1;
                        tanU1 = (K1 * obj.objHeight - K2 * pupilDiameter / 2) / obj.objDistance;
                        beam.Add(K1.ToString() + "  " + K2.ToString(), new Beam(K2 * pupilDiameter / 2 / tanU1,
                            Math.Atan(tanU1)));
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

        //Calculate the ideal height of the image, for d-light only
        private Dictionary<string, double> idealH(Dictionary<string, Beam> outputGaussian)
        {
            Dictionary<string, double> tmp = new Dictionary<string, double>();
            double beta = -(outputGaussianOn["d"].l - basicPoints["lH"] - basicPoints["fp"]) / basicPoints["fp"];
            double idealH;
            foreach (KeyValuePair<string, Beam> kvp in outputGaussian)

            {
                string str = kvp.Key;
                str = str.Substring(str.Length - 1, 1);
                if (str == "d")
                {
                    if (isInfinite)
                    {
                        idealH = Math.Tan(double.Parse(kvp.Key.Substring(0, kvp.Key.Length - 3)) * obj.fieldAngle) * basicPoints["fp"];
                        tmp.Add(kvp.Key, idealH);
                    }
                    else
                    {
                        idealH = beta * obj.objHeight;
                        tmp.Add(kvp.Key, idealH);
                    }
                }
                else continue;
            }

            return tmp;
        }

        //Calculate the real height of the image, for d-light,F-light and C-lgiht
        private Dictionary<string, double> realH(Dictionary<string, Beam> outputGaussian)
        {
            Dictionary<string, double> tmp = new Dictionary<string, double>();

            double realH;

            foreach (KeyValuePair<string, Beam> kvp2 in outputGaussian)
            {
                string str = kvp2.Key;
                str = str.Substring(str.Length - 1, 1);
                realH = (outputGaussianOn[str].l - kvp2.Value.l) * Math.Tan(kvp2.Value.u);
                tmp.Add(kvp2.Key, realH);
                //}
            }


            return tmp;
        }

        //Calculate the basic points, for d-light only
        private Dictionary<string, double> calBasicPoints()
        {
            Beam incident_tmp;
            Queue<double> RefractiveIndex = new Queue<double>(RIndexOn["d"]);
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


        private astigBeam ImageDiffRefraction(astigBeam incidentBeam1, Queue<double> Radius,
            Queue<double> RefractiveIndex, Queue<double> Interval)
        {
            
            double s1p, t1p, s2, t2, PA1,X1,PA2, X2, D1;
            double incidentAngle, exitAngle;
            double radius = Radius.Dequeue();
            double n = RefractiveIndex.Dequeue();
            double np = RefractiveIndex.Peek();

            //double s1 = t1 = y / Math.Sin(incidentBeam1.u);

            incidentAngle = Math.Asin((incidentBeam1.l - radius) * Math.Sin(incidentBeam1.u) / radius);
            exitAngle = Math.Asin(n * Math.Sin(incidentAngle) / np);

            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);

            s1p = np / ((np * Math.Cos(exitAngle) - n * Math.Cos(incidentAngle)) / radius + n / incidentBeam1.s);
            t1p = np * Math.Cos(exitAngle) * Math.Cos(exitAngle) / ((np * Math.Cos(exitAngle) -
                n * Math.Cos(incidentAngle)) / radius + n * Math.Cos(incidentAngle) * Math.Cos(incidentAngle) / incidentBeam1.t);
            double u1p = incidentAngle + incidentBeam1.u - exitAngle;
            double l1p = radius + radius * Math.Sin(exitAngle) / Math.Sin(u1p);

             
            astigBeam exitBeam1 = new astigBeam(l1p, u1p, s1p, t1p,incidentBeam1.x);
            if (Radius.Count == 0) return exitBeam1; // the last sphere does not have any sphere behind
            
            double u2 = u1p;
            double l2 = l1p - Interval.Peek();
            double tmp = Math.Asin((l2 - Radius.Peek()) * Math.Sin(u2) / Radius.Peek());
            PA2 = l2 * Math.Sin(u1p) / Math.Cos((Math.Asin(tmp - u2) / 2));
            X2 = PA2 * PA2 / 2 / radius;
            D1 = (Interval.Dequeue() - incidentBeam1.x + X2) / Math.Cos(u1p);
            t2 = t1p - D1;
            s2 = s1p - D1;

            astigBeam incidentBeam2 = new astigBeam(l2, u2, s2, t2, X2);

            return ImageDiffRefraction(incidentBeam2, Radius, RefractiveIndex, Interval);
        }
    }
}
