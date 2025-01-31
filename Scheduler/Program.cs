using Scheduler.Enums;
using Scheduler.SharedResourceMeneger.Services.SchedulerService;

namespace Scheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunNonPreemptive();
            Console.ReadLine();
        }
        public static void RunNonPreemptive()
        {
            PreemptiveTaskScheduler coreTaskScheduler = new PreemptiveTaskScheduler(2);
            CooperationMechanizm cooperationMechanism1 = new CooperationMechanizm();
            void action1()
            {
                for (int i = 0; i < 10; ++i)
                {
                    Console.WriteLine("Action 1 is executing step " + i);
                    Thread.Sleep(200);
                    if (cooperationMechanism1.IsCancelled)
                    {
                        Console.WriteLine("Action 1 is cancelled");
                        break;
                    }
                }
            }

            CooperationMechanizm cooperationMechanism2 = new CooperationMechanizm();
            void action2()
            {
                for (int i = 0; i < 15; ++i)
                {
                    Console.WriteLine("Action 2 is executing step " + i);
                    Thread.Sleep(200);
                    if (cooperationMechanism2.IsCancelled)
                    {
                        Console.WriteLine("Action 2 is cancelled");
                        break;
                    }
                }
            }

            CooperationMechanizm cooperationMechanism3 = new CooperationMechanizm();
            void action3()
            {
                for (int i = 0; i < 15; ++i)
                {
                    Console.WriteLine("Action 3 is executing step " + i);
                    Thread.Sleep(200);
                    if (cooperationMechanism3.IsCancelled)
                    {
                        Console.WriteLine("Action 3 is cancelled");
                        return;
                    }
                }
            }


            CooperationMechanizm cooperationMechanism4 = new CooperationMechanizm();
            void action4()
            {
                for (int i = 0; i < 5; ++i)
                {
                    Console.WriteLine("Action 4 is executing step " + i);
                    Thread.Sleep(200);
                    if (cooperationMechanism4.IsCancelled)
                    {
                        Console.WriteLine("Action 4 is cancelled");
                        break;
                    }
                }
            }
            coreTaskScheduler.QueueForScheduling(new List<PrioritizedLimitedTask>()
                           {
                                     new PrioritizedLimitedTask(action1, Priority.High, 2000)
                                          {
                                           CooperationMechanism = cooperationMechanism1
                                           },
                                     new PrioritizedLimitedTask(action2, Priority.High, 3000)
                                          {
                                           CooperationMechanism = cooperationMechanism2
                                          },
                                      new PrioritizedLimitedTask(action3, Priority.Normal, 3000)
                                           {
                                           CooperationMechanism = cooperationMechanism3
                                      }
                            });
            PrioritizedLimitedTask prioritizedLimitedTask = new PrioritizedLimitedTask(action4, Priority.Highest, 1000)
            {
                CooperationMechanism = cooperationMechanism4
            };
            // Real-time scheduling
            prioritizedLimitedTask.Start(coreTaskScheduler);
            
        }
    }
  
}
    