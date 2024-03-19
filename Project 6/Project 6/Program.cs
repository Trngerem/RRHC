// See https://aka.ms/new-console-template for more information

List<Double> data = readData("data.csv"); //reference to the data.csv file doc
static List<Double> readData(string filename)
{
    List<Double> res = new List<double>();

    string[] x = System.IO.File.ReadAllLines(filename);
    var raw = x[0].Split(',');

    foreach (var item in raw)
    {
        res.Add(Convert.ToDouble(item));
    }
    return res;
} //this point we have the data in a list

List<Double> weights = new List<Double>();
Random random = new Random();

int index;
int totaldata = 29;

for (int i = 0; i < 20; i++)
{
    index = random.Next(0, totaldata);
    double tobeadded = data[index];
    weights.Add(tobeadded);
    data.Remove(tobeadded);
    totaldata--;
}//we have our list of 20 weights

List<double> bestsolutions = RandomRestartHillClimbingAlgorithm(weights);
printList(bestsolutions);

static void printvan(List<double> list)
{
    foreach (double i in list)
    {
        Console.Write(i + ", ");
    }
    Console.WriteLine("");
}//prints whole list

static void printList(List<double> list)
{
    foreach (double i in list)
    {
        Console.WriteLine(i);
        Console.WriteLine("<==>");

    }
}//prints whole list

static List<double> RandomRestartHillClimbingAlgorithm(List<double> assign)
{
    List<double> solutions = new List<double>();
    double bestfitness = 99999;

    for (int i = 0; i < 50; i++)//iterate this many tests
    {
        List<double> van1 = new List<double>();
        List<double> van2 = new List<double>();
        List<double> van3 = new List<double>();

        List<double> weights = new List<double>();
        Random random = new Random();

        int totaldata = assign.Count;
        List<double> assignment = new List<double>(assign); // Make a copy to avoid modifying the original list

        for (int x = 0; x < 20 && totaldata > 0; x++)
        {
            int index = random.Next(0, totaldata);
            double tobeadded = assignment[index];
            weights.Add(tobeadded);
            assignment.RemoveAt(index);
            totaldata--;
        }

        for (int j = 0; j < weights.Count; j++) //asssigning the weights
        {

            if (j == 0)
            {
                van1.Add(weights[j]); //assign one value to each to start with
            }
            if (j == 1)
            {
                van2.Add(weights[j]);
            }
            if (j == 2)
            {
                van3.Add(weights[j]);
            }

            double total1 = calculatetotal(van1);
            double total2 = calculatetotal(van2);
            double total3 = calculatetotal(van3);

            if (j >= 3)
            {
                if (Math.Min(total1, total2) == Math.Min(total1, total3))
                {
                    van1.Add(weights[j]);

                }
                if (Math.Min(total2, total1) == Math.Min(total2, total3))
                {
                    van2.Add(weights[j]);

                }
                if (Math.Min(total3, total2) == Math.Min(total3, total1))
                {
                    van3.Add(weights[j]);
                }

                if (j == weights.Count() - 1)
                {
                    for (int k = 0; k < 2; k++) //small change function
                    {
                        double totals1 = calculatetotal(van1);
                        double totals2 = calculatetotal(van2);
                        double totals3 = calculatetotal(van3);//totals

                        List<double> doubles = new List<double>();
                        doubles.Add(totals1);
                        doubles.Add(totals2);
                        doubles.Add(totals3);

                        double biggesttotal = doubles.Max();
                        doubles.Remove(doubles.Max()); //biggest total

                        double smallesttotal = doubles.Min();
                        doubles.Remove(doubles.Min()); //smallest total
                                                       //doubles contains total of middlelist

                        if (totals1 == doubles.Min())
                        {
                            if (smallesttotal == total2)
                            {
                                if (biggesttotal - (smallesttotal + van1.Min()) < calculateFitness(van1, van2, van3))
                                {
                                    double temp = van1.Min();
                                    van1.Remove(van1.Min());
                                    van2.Add(temp);
                                }
                            }
                            else if (smallesttotal == total3)
                            {
                                if (biggesttotal - (smallesttotal + van1.Min()) < calculateFitness(van1, van2, van3))
                                {
                                    double temp = van1.Min();
                                    van1.Remove(van1.Min());
                                    van3.Add(temp);
                                }
                            }

                        }
                        else if (totals2 == doubles.Min())
                        {
                            if (smallesttotal == total1)
                            {
                                if (biggesttotal - (smallesttotal + van2.Min()) < calculateFitness(van1, van2, van3))
                                {
                                    double temp = van2.Min();
                                    van2.Remove(van2.Min());
                                    van1.Add(temp);
                                }
                                else if (smallesttotal == total3)
                                {
                                    if (biggesttotal - (smallesttotal + van2.Min()) < calculateFitness(van1, van2, van3))
                                    {
                                        double temp = van2.Min();
                                        van2.Remove(van2.Min());
                                        van3.Add(temp);
                                    }
                                }
                            }
                        }
                        else if (totals3 == doubles.Min())
                        {
                            if (smallesttotal == total1)
                            {
                                if ((biggesttotal - (smallesttotal + van2.Min())) < calculateFitness(van1, van2, van3))
                                {
                                    double temp = van3.Min();
                                    van3.Remove(van3.Min());
                                    van1.Add(temp);
                                }
                                else if (smallesttotal == total2)
                                {
                                    if ((biggesttotal - (smallesttotal + van2.Min())) < calculateFitness(van1, van2, van3))
                                    {
                                        double temp = van3.Min();
                                        van3.Remove(van3.Min());
                                        van2.Add(temp);
                                    }
                                }
                            }

                        }
                    }

                    double currentfitness = calculateFitness(van1, van2, van3);
                    solutions.Add(currentfitness);

                    if (currentfitness < bestfitness)
                    {
                        Console.Write("Improving fitness : ");
                        Console.WriteLine(currentfitness);
                        bestfitness = currentfitness;

                        Console.WriteLine("");
                        Console.WriteLine("van1");
                        Console.WriteLine("----");
                        printvan(van1);
                        Console.WriteLine("");
                        Console.WriteLine("van2");
                        Console.WriteLine("----");
                        printvan(van2);
                        Console.WriteLine("");
                        Console.WriteLine("van3");
                        Console.WriteLine("----");
                        printvan(van3);
                        Console.WriteLine("<==>");
                        Console.WriteLine("");
                        Console.WriteLine("-------------------------------");

                    }
                }

            }

        }
    }

    return solutions;
}//random restart hill climb



/*static void smallchange(List<double> van1, List<double> van2, List<double> van3)
{

    for (int k = 0; k < 10; k++)
    {
        double total1 = calculatetotal(van1);
        double total2 = calculatetotal(van2);
        double total3 = calculatetotal(van3);//totals

        List<double> doubles = new List<double>();
        doubles.Add(total1);
        doubles.Add(total2);
        doubles.Add(total3);

        double biggesttotal = doubles.Max();
        doubles.Remove(doubles.Max()); //biggest total

        double smallesttotal = doubles.Min();
        doubles.Remove(doubles.Min()); //smallest total
                                       //doubles contains total of middlelist

        if (total1 == doubles.Min())
        {
            if (smallesttotal == total2)
            {
                if (biggesttotal - (smallesttotal + van1.Min()) < biggesttotal - smallesttotal)
                {
                    double temp = van1.Min();
                    van1.Remove(van1.Min());
                    van2.Add(temp);
                }
            }
            else if (smallesttotal == total3)
            {
                if (biggesttotal - (smallesttotal + van1.Min()) < biggesttotal - smallesttotal)
                {
                    double temp = van1.Min();
                    van1.Remove(van1.Min());
                    van3.Add(temp);
                }
            }

        }
        else if (total2 == doubles.Min())
        {
            if (smallesttotal == total1)
            {
                if (biggesttotal - (smallesttotal + van2.Min()) < biggesttotal - smallesttotal)
                {
                    double temp = van2.Min();
                    van2.Remove(van2.Min());
                    van1.Add(temp);
                }
                else if (smallesttotal == total3)
                {
                    if (biggesttotal - (smallesttotal + van2.Min()) < biggesttotal - smallesttotal)
                    {
                        double temp = van2.Min();
                        van2.Remove(van2.Min());
                        van3.Add(temp);
                    }
                }
            }
        }
        else if (total3 == doubles.Min())
        {
            if (smallesttotal == total1)
            {
                if ((biggesttotal - (smallesttotal + van2.Min())) < (biggesttotal - smallesttotal))
                {
                    double temp = van3.Min();
                    van3.Remove(van3.Min());
                    van1.Add(temp);
                }
                else if (smallesttotal == total2)
                {
                    if ((biggesttotal - (smallesttotal + van2.Min())) < (biggesttotal - smallesttotal))
                    {
                        double temp = van3.Min();
                        van3.Remove(van3.Min());
                        van2.Add(temp);
                    }
                }
            }

        }
    }
}
*/
static double calculatetotal(List<double> list)
{
    double totaldata = 0;

    foreach (double v in list)
    {
        totaldata = totaldata + v;
    }

    return totaldata;
}//calc total of a lists values

static double calculateFitness(List<double> list1, List<double> list2, List<double> list3)
{
    double lorry1 = calculatetotal(list1);
    double lorry2 = calculatetotal(list2);
    double lorry3 = calculatetotal(list3);

    largesttakesmallest(lorry1, lorry2, lorry3);

    double fitness = largesttakesmallest(lorry1, lorry2, lorry3);

    return fitness;
}//finds fitness of 3 values (largest - smallest)

static double largesttakesmallest(double lorry1, double lorry2, double lorry3)
{
    double fitness = 0;

    List<double> doubles = new List<double>();
    doubles.Add(lorry1);
    doubles.Add(lorry2);
    doubles.Add(lorry3);

    double biggest = doubles.Max();
    doubles.Remove(doubles.Max());

    double smallest = doubles.Min();
    doubles.Remove(doubles.Min());

    fitness = biggest - smallest;

    return fitness;
}
