using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter with the first Input consider 2 numbers split by blank ex: 9 2");
            string firstInput = Console.ReadLine();
            int[] array = null;
            bool flag = true;
            string founder = string.Empty;
            do
            {
                if (String.IsNullOrEmpty(firstInput))
                {
                    Console.WriteLine("Input field is null or empty try again");
                    firstInput = Console.ReadLine();
                }
                else
                {
                    var inputSplit = firstInput.Split(' ');
                    if (!(inputSplit.Length > 1))
                    {


                        Console.WriteLine("Input field incorrect try again");
                        firstInput = Console.ReadLine();
                    }
                    else
                    {
                        flag = false;
                        array = inputSplit.Select(Int32.Parse).ToArray();
                    }

                }
            }
            while (flag);
            Console.WriteLine("Input the founder name");
            founder = Console.ReadLine();
            do
            {
                flag = true;
                if (String.IsNullOrEmpty(founder))
                {
                    Console.WriteLine("The founder name cannot be null or empty");
                    Console.WriteLine("Input the founder name");
                    founder = Console.ReadLine();
                }
                else
                {
                    flag = ValidStringAZ(founder);
                    if (flag == false)
                    {
                        Console.WriteLine("The founder name have to be regular expressions");
                        Console.WriteLine("Input the founder name");
                        founder = Console.ReadLine();
                        flag = true;
                    }

                    else
                        flag = false;
                }
            } while (flag);
            List<Names> familyRelation = new List<Names>();
            List<ClaimThrone> claimThrone = new List<ClaimThrone>();
            for (int i = 0; i < (int)array[0]; i++)
            {
                flag = true;
                string fullName = string.Empty;
                Console.WriteLine("Input the full family name containing 3 names split by blank space, ex: name1 name2 name3");
                fullName = Console.ReadLine();
                do
                {

                    if (String.IsNullOrEmpty(fullName))
                    {
                        Console.WriteLine("Input field is null or empty try again");
                        fullName = Console.ReadLine();
                    }
                    else
                    {
                        var inputSplit = fullName.Split(' ').ToArray();
                        if (!(inputSplit.Length > 2 && inputSplit.Length <= 3 ))
                        {
                            Console.WriteLine("Input field incorrect try again");
                            firstInput = Console.ReadLine();
                        }
                        if(inputSplit.Count() > 0)
                        {
                            for (int j = 0; j < inputSplit.Count(); j++)
                            {

                                if (!ValidStringAZ(inputSplit[j]) || !MaxLenght(inputSplit[j]))
                                {
                                    Console.WriteLine("The name have to be regular expressions and max lenght per name is 10");
                                    fullName = Console.ReadLine();
                                    flag = true;
                                    break;
                                }
                                else
                                    flag = false;

                            }

                            if (flag == false)
                            {
                                
                                Names current = new Names
                                {
                                    FirstName = inputSplit[0].ToLower(), MiddleName = inputSplit[1].ToLower(),
                                    LastName = inputSplit[2].ToLower(), Position = i
                                };

                                Names parent = familyRelation.Where(p => (p.FirstName == current.MiddleName) || (p.FirstName == current.LastName)).FirstOrDefault();
                                if (parent is null)
                                {
                                    //FILHO FOUNDER
                                    current.RelatedScore = 1;
                                }
                                else
                                {
                                    //FILHO DO PARENT
                                    current.RelatedScore = parent.RelatedScore + 1;
                                }
                                familyRelation.Add(current);
                            }
                        }

                    }

                } while (flag);
            }
            for (int i = 0; i < (int)array[1]; i++)
            {
                flag = true;
                string claiming = string.Empty;
                Console.WriteLine("Input the name of who claim the throune");
                claiming = Console.ReadLine();
                do
                {
                    if (String.IsNullOrEmpty(claiming))
                    {
                        Console.WriteLine("Input field is null or empty try again");
                        claiming = Console.ReadLine();
                    }
                    else if(!MaxLenght(claiming) || !ValidStringAZ(claiming))
                    {
                        Console.WriteLine("Input field is null or empty try again");
                        claiming = Console.ReadLine();
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                        claimThrone.Add(new ClaimThrone { Name = claiming.ToLower() });
                        
                        Names nameCurrent = familyRelation.Where(p => p.FirstName == claiming.ToLower()).FirstOrDefault();

                        if (nameCurrent != null)
                        {
                            nameCurrent.Claim = true;
                            nameCurrent.ClaimPosition = i;
                        }
                         
                    }
                        

                } while (flag);
            }

            Console.WriteLine("\n\n" + FindNewKing3(familyRelation));

        }
        public static bool ValidStringAZ(string input)
        {
            if (!Regex.IsMatch(input, @"^[a-zA-Z]+$"))
                return false;
            return true;
        }
        public static bool MaxLenght(string input)
        {
            if (input.Length > 10)
                return false;
            return true;
        }


        public static string FindNewKing3(List<Names> familyRelation)
        {
            Names bestFounder = new Names();

            foreach (var familyCurrent in familyRelation)
            {
                if (familyCurrent.Claim && familyCurrent.RelatedScore >= bestFounder.RelatedScore && familyCurrent.ClaimPosition > bestFounder.ClaimPosition)
                    bestFounder = familyCurrent;
            }
            
            return bestFounder.FirstName;
        }

        public class Names
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public int RelatedScore { get; set; } = 0;
            public bool Complete { get; set; }

            public int Position { get; set; } = 0;
            public int ClaimPosition { get; set; } = 0;
            public bool Claim { get; set; } = false;


        }
        public class ClaimThrone
        {
            public string Name { get; set; }
        }
    }
}
