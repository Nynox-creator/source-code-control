using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;


namespace nynox_manage_prisma_master_console
{
    class Program
    {
        static String RollUpData = ConfigurationManager.AppSettings["RollUpData"];
        static String ResultRollUpTargetFile = ConfigurationManager.AppSettings["ResultRollUpTargetFile"];
        static String ResultRollUpData = ConfigurationManager.AppSettings["ResultRollUpData"];

        static AccountCollection Accounts = new AccountCollection();

        static AccountHierarchyCollection AccountHierarchies = new AccountHierarchyCollection();

        static void Main(string[] args)
        {
            ProcessProductAttributes();
            ProcessProductHierarchy();
            ProductDescription();
            ProductDesiredResult();
            FinalResult();
            Console.WriteLine("Done");
            Console.ReadLine();

        }

        static void ProcessProductAttributes()
        {
            //Read Attributes
            if (File.Exists(RollUpData))
            {
                string[] lines = File.ReadAllLines(RollUpData);

                foreach (string line in lines)
                {
                    /******************************/
                    string[] Split = line.Split(',');

                    Account account = new Account();


                    account.group_account_code = Split[0].Trim();
                    account.group_account_name = Split[1].Trim();
                    account.detail_account_code = Split[2].Trim();
                    account.detail_account_name = Split[3].Trim();
                    account.detail = Split[4].Trim();
                    account.calculate_in_report = Split[5].Trim();
                    account.listing_order = Split[6].Trim();

                    Accounts.Add(account);
                }
            }
        }

        // Process Product Hierarchy
        static void ProcessProductHierarchy()
        {

            //Read Attributes
            if (File.Exists(RollUpData))
            {
                string[] lines = File.ReadAllLines(RollUpData);

                foreach (string line in lines)
                {
                    /******************************/

                    string[] Split = line.Split(',');

                    string LabelParent = Split[0].Trim();
                    string LabelChild = Split[2].Trim();

                    AccountHierarchy AccountHierarchy = new AccountHierarchy();

                    // Get Parent Object
                    foreach (Account p in Accounts)
                    {
                        if (p.detail_account_code == LabelParent)
                        {
                            //find whether parent exist in hierarchy
                            bool IsParentFound = false;

                            foreach (AccountHierarchy h in AccountHierarchies)
                            {
                                if (h.Parent.detail_account_code == LabelParent)
                                {
                                    AccountHierarchy = h;
                                    IsParentFound = true;
                                    break;
                                }
                            }

                            if (!IsParentFound)
                            {
                                AccountHierarchy.Parent = p;

                                AccountHierarchies.Add(AccountHierarchy);
                            }

                            break;
                        }
                    }

                    // Get Child Object
                    foreach (Account p in Accounts)
                    {
                        if (p.detail_account_code == LabelChild)
                        {
                            AccountHierarchy.Children.Add(p);
                            break;
                        }
                    }
                }
            }

        }
        static void ProductDescription()
        {
            //Write Output Result

            using (StreamWriter writer = new StreamWriter(ConfigurationManager.AppSettings["ResultRollUpHierarchyFile"]))
            {
                foreach (AccountHierarchy h in AccountHierarchies)
                {
                    foreach (Account p in h.Children)
                    {
                        writer.WriteLine(h.Parent.group_account_code + ";" + p.group_account_name + ";" + p.detail_account_code + ";" + h.Parent.detail_account_name + ";" + h.Parent.detail + ";" + h.Parent.calculate_in_report + ";" + h.Parent.listing_order);

                    }
                }
            }
        }

        static void ProductDesiredResult()
        {
            List<string> productPrisma = new List<string>();

            using (StreamWriter writer = new StreamWriter(ConfigurationManager.AppSettings["ResultRollUpTargetFile"]))
            {
                foreach (AccountHierarchy h in AccountHierarchies)
                {
                    //if (h.Parent.detail_account_code == "detail_account_code" || h.Parent.group_account_code == "group_account_code")
                    //{
                    Console.WriteLine(h.Parent.detail_account_code);
                    string level1 = h.Parent.detail_account_code;

                    foreach (Account p1 in h.Children) // child of spirits
                    {
                        string level2 = p1.detail_account_code;
                        level2 = level1 + ";" + level2;

                        foreach (AccountHierarchy h1 in AccountHierarchies)
                        {
                            if (h1.Parent.detail_account_code == p1.detail_account_code)
                            {
                                // child of 'ANIS'
                                foreach (Account p2 in h1.Children)
                                {
                                    string level3 = p2.detail_account_code;
                                    level3 = level2 + ";" + level3;

                                    foreach (AccountHierarchy h2 in AccountHierarchies)
                                    {
                                        if (h2.Parent.detail_account_code == p2.detail_account_code)
                                        {
                                            // child of 'PASTIS'
                                            foreach (Account p3 in h2.Children)
                                            {
                                                string level4 = p3.detail_account_code;
                                                level4 = level3 + ";" + level4;

                                                foreach (AccountHierarchy h3 in AccountHierarchies)
                                                {
                                                    if (h3.Parent.detail_account_code == p3.detail_account_code)
                                                    {
                                                        //child of 'TOTAL_RICARD'
                                                        foreach (Account p4 in h3.Children)
                                                        {
                                                            string level5 = p4.detail_account_code;
                                                            level5 = level4 + ";" + level5;

                                                            foreach (AccountHierarchy h4 in AccountHierarchies)
                                                            {
                                                                if (h4.Parent.detail_account_code == p4.detail_account_code)
                                                                {
                                                                    //child of 'RICARD'
                                                                    foreach (Account p5 in h4.Children)
                                                                    {
                                                                        string level6 = p5.detail_account_code;
                                                                        level6 = level5 + ";" + level6;

                                                                        // writer.WriteLine(level6);

                                                                        productPrisma.Add(level6);

                                                                        foreach (AccountHierarchy h5 in AccountHierarchies)
                                                                        {
                                                                            if (h5.Parent.detail_account_code == p5.detail_account_code)
                                                                            {
                                                                                //child of ''
                                                                                foreach (Account p6 in h5.Children)
                                                                                {
                                                                                    string level7 = p6.detail_account_code;
                                                                                    level7 = level6 + ";" + level7;

                                                                                    //  writer.WriteLine(level7);

                                                                                    //      productPrisma.Add(level7);

                                                                                    foreach (AccountHierarchy h6 in AccountHierarchies)
                                                                                    {
                                                                                        if (h6.Parent.detail_account_code == p6.detail_account_code)
                                                                                        {
                                                                                            // child of ''
                                                                                            foreach (Account p7 in h6.Children)
                                                                                            {
                                                                                                // Samllest child of the hierarchy 
                                                                                                string level8 = p7.detail_account_code;

                                                                                                level8 = level7 + ";" + level8;
                                                                                                if (level8.StartsWith("RSTT30"))
                                                                                                {
                                                                                                    string[] sArray = level8.Split(';');
                                                                                                    List<string> sList = new List<string>();

                                                                                                    for (int i = 0; i < sArray.Length; i++)
                                                                                                    {
                                                                                                        if (sList.Contains(sArray[i]) == false)
                                                                                                        {
                                                                                                            sList.Add(sArray[i]);
                                                                                                        }
                                                                                                    }
                                                                                                    foreach (string str in sList)
                                                                                                    {
                                                                                                        writer.Write(str + ";");
                                                                                                    }
                                                                                                    writer.WriteLine();
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Product result
        }
        static void FinalResult()
        {
            List<string> emailAddresses = new List<string>();
            using (StringReader reader = new StringReader(File.ReadAllText(ResultRollUpTargetFile)))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                    if (!emailAddresses.Contains(line))
                        emailAddresses.Add(line);
            }
            using (StreamWriter writer = new StreamWriter(File.Open(ResultRollUpData, FileMode.Create)))
                foreach (string value in emailAddresses)
                {
                    string[] splitting = value.Split(';');
                    
                            if (splitting.Length > 3)
                            {
                                 foreach (AccountHierarchy h in AccountHierarchies)
                                    {
                                        foreach (Account p in h.Children)
                                        {
                                            foreach(string str in splitting)
                                            {

                                            }                               
                                              
                                        }
                            writer.WriteLine();
                                }
                              
                            }
                }                   
        }
    }
}
