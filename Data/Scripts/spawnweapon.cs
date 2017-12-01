using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRage.Game.Entity;
using VRage.Voxels;

namespace Douxt
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Assembler), true, "BlackShop")]
    public class BlackShop : MyGameLogicComponent
    {
        private MyObjectBuilder_EntityBase builder;
        private Sandbox.ModAPI.IMyAssembler m_generator;
        private IMyCubeBlock m_parent;

        Sandbox.ModAPI.IMyTerminalBlock terminalBlock;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            m_generator = Entity as Sandbox.ModAPI.IMyAssembler;
            m_parent = Entity as IMyCubeBlock;
            builder = objectBuilder;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            terminalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }
        #region UpdateBeforeSimulation
        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();

            if (m_generator.IsWorking)
            {

                IMyInventory inventory = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(1) as IMyInventory;
                VRage.MyFixedPoint amount = 1;

                if (!inventory.ContainItems(amount, new MyObjectBuilder_PhysicalGunObject { SubtypeName = "AutomaticRifleItem" }))
                {
                    inventory.AddItems(amount, new MyObjectBuilder_PhysicalGunObject { SubtypeName = "AutomaticRifleItem" });
                    terminalBlock.RefreshCustomInfo();
                }
                if (!inventory.ContainItems(amount, new MyObjectBuilder_AmmoMagazine { SubtypeName = "NATO_5p56x45mm" }))
                {
                    inventory.AddItems(10, new MyObjectBuilder_AmmoMagazine { SubtypeName = "NATO_5p56x45mm" });
                    terminalBlock.RefreshCustomInfo();
                }
                //if (!inventory.ContainItems(10000, new MyObjectBuilder_Ore { SubtypeName = "Iron" }))
                //{
                //    inventory.AddItems(amount, new MyObjectBuilder_Ore { SubtypeName = "Iron" });
                //    terminalBlock.RefreshCustomInfo();
                //}

                float cur = (float)inventory.CurrentVolume;
                float max = (float)inventory.MaxVolume;

                if (cur / max < 0.9 && inventory.ContainItems(1, new MyObjectBuilder_Ingot { SubtypeName = "Supply" }))
                {
                    inventory.RemoveItemsOfType(1, new MyObjectBuilder_Ingot { SubtypeName = "Supply" });

                    BlackShop.AddSupply(inventory);
                    terminalBlock.RefreshCustomInfo();
                }

                if (cur / max < 0.9 && inventory.ContainItems(1, new MyObjectBuilder_Ingot { SubtypeName = "Supply2" }))
                {
                    inventory.RemoveItemsOfType(1, new MyObjectBuilder_Ingot { SubtypeName = "Supply2" });

                    BlackShop.AddSupply(inventory, true);
                    terminalBlock.RefreshCustomInfo();
                }

            }
        }
        #endregion
        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return builder;
        }

        public static void AddSupply(IMyInventory inventory, bool spider = false)
        {
            Random ran = new Random();
            int num = ran.Next(1, 12);
            int amount = 3000;
            String type = "Iron";
            switch (num)
            {
                case 1:
                    amount = amount * 2;
                    type = "Stone";
                    break;
                case 2:
                    amount = amount / 5;
                    type = "Nickel";
                    break;
                case 3:
                    amount = amount / 5;
                    type = "Cobalt";
                    break;
                case 4:
                    amount = amount / 5;
                    type = "Magnesium";
                    break;
                case 5:
                    amount = amount / 4;
                    type = "Silicon";
                    break;
                case 6:
                    amount = amount / 10;
                    type = "Silver";
                    break;
                case 7:
                    amount = amount / 15;
                    type = "Gold";
                    break;
                case 8:
                    amount = amount / 15;
                    type = "Uranium";
                    break;
                case 9:
                case 10:
                    if (spider)
                    {
                        amount = amount / 5;
                        type = "Platinum";
                    }
                    break;

                default:
                    break;
            }
            inventory.AddItems(ran.Next(50, amount), new MyObjectBuilder_Ore { SubtypeName = type });

            num = ran.Next(1, 100);
            if (num < 10)
            {
                inventory.AddItems(ran.Next(1, 100), new MyObjectBuilder_Ingot { SubtypeName = "Coin" });
            }
            else if (num < 50)
            {
                inventory.AddItems(ran.Next(1, 10), new MyObjectBuilder_Ingot { SubtypeName = "Coin" });
            }
			
			int he = 0;
			if (spider)
			{
				he = 3;
			}
			
			inventory.AddItems(ran.Next(1 + he, 3 + he), new MyObjectBuilder_Ingot { SubtypeName = "Helium" });
        }

    }
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Assembler), true, "IronMine")]
    public class IronMine : MyGameLogicComponent
    {
        private MyObjectBuilder_EntityBase builder;
        private Sandbox.ModAPI.IMyAssembler m_generator;
        private IMyCubeBlock m_parent;

        Sandbox.ModAPI.IMyTerminalBlock terminalBlock;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            m_generator = Entity as Sandbox.ModAPI.IMyAssembler;
            m_parent = Entity as IMyCubeBlock;
            builder = objectBuilder;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            terminalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }
        #region UpdateBeforeSimulation
        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();

            if (m_generator.IsFunctional)
            {
                IMyInventory inventory = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(1) as IMyInventory;
                VRage.MyFixedPoint amount = 1;

                if (!inventory.ContainItems(10000, new MyObjectBuilder_Ore { SubtypeName = "Iron" }))
                {
                    inventory.AddItems(5, new MyObjectBuilder_Ore { SubtypeName = "Iron" });
                    terminalBlock.RefreshCustomInfo();
                }
            }


        }
        #endregion
        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return builder;
        }

    }
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Assembler), true, "RandomShop")]
    public class RandomShop : MyGameLogicComponent
    {
        private MyObjectBuilder_EntityBase builder;
        private Sandbox.ModAPI.IMyAssembler m_generator;
        private IMyCubeBlock m_parent;

        Sandbox.ModAPI.IMyTerminalBlock terminalBlock;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            m_generator = Entity as Sandbox.ModAPI.IMyAssembler;
            m_parent = Entity as IMyCubeBlock;
            builder = objectBuilder;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            terminalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }
        #region UpdateBeforeSimulation
        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();

            if (m_generator.IsFunctional)
            {
                IMyInventory inventory = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(1) as IMyInventory;
                VRage.MyFixedPoint amount = 1;
                float cur = (float)inventory.CurrentVolume;
                float max = (float)inventory.MaxVolume;

                if (cur / max < 0.8 && inventory.ContainItems(1, new MyObjectBuilder_Ingot { SubtypeName = "RandomItem" }))
                {
                    inventory.RemoveItemsOfType(1, new MyObjectBuilder_Ingot { SubtypeName = "RandomItem" });

                    RandomShop.AddRandomItem(inventory);
                    terminalBlock.RefreshCustomInfo();
                }
            }


        }
        #endregion
        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return builder;
        }

        public static void AddRandomItem(IMyInventory inventory)
        {
            Random ran = new Random();
            int num = ran.Next(1, 30);
            int amount = 500;
            String type = "SteelPlate";
            switch (num)
            {
                case 1:
                    amount = amount * 2;
                    type = "Construction";
                    break;
                case 2:
                    amount = amount / 5;
                    type = "MetalGrid";
                    break;
                case 3:
                    amount = amount * 5;
                    type = "InteriorPlate";
                    break;
                case 4:
                    amount = amount * 3;
                    type = "Girder";
                    break;
                case 5:
                    amount = amount * 4;
                    type = "SmallTube";
                    break;
                case 6:
                    amount = amount / 2;
                    type = "LargeTube";
                    break;
                case 7:
                    amount = amount / 2;
                    type = "Motor";
                    break;
                case 8:
                    amount = amount / 2;
                    type = "Display";
                    break;
                case 9:
                    amount = amount / 3;
                    type = "BulletproofGlass";
                    break;
                case 10:
                    amount = amount / 5;
                    type = "Superconductor";
                    break;
                case 11:
                    amount = 5000;
                    type = "Computer";
                    break;
                case 12:
                    amount = amount / 6;
                    type = "Reactor";
                    break;
                //case 13:
                //    amount = 50;
                //    type = "Thrust";
                //    break;
                case 14:
                    amount = amount / 10;
                    type = "GravityGenerator";
                    break;
                case 15:
                    amount = amount / 40;
                    type = "Medical";
                    break;
                case 16:
                    amount = 5;
                    type = "RadioCommunication";
                    break;
                case 17:
                    amount = amount / 4;
                    type = "Detector";
                    break;
                case 18:
                    amount = amount / 5;
                    type = "Explosives";
                    break;
                case 19:
                    amount = amount / 4;
                    type = "SolarCell";
                    break;
                case 20:
                    amount = amount / 2;
                    type = "PowerCell";
                    break;
                case 21:
                    amount = amount / 8;
                    type = "Canvas";
                    break;
                default:
                    break;
            }
            inventory.AddItems(ran.Next(1, amount), new MyObjectBuilder_Component { SubtypeName = type });

            num = ran.Next(1, 100);
            if (num < 10)
            {
                inventory.AddItems(ran.Next(1, 100), new MyObjectBuilder_Ingot { SubtypeName = "Coin" });
            }
            else if (num < 50)
            {
                inventory.AddItems(ran.Next(1, 10), new MyObjectBuilder_Ingot { SubtypeName = "Coin" });
            }
        }

    }

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_CargoContainer), true, "FuBox")]
    public class FuBox : MyGameLogicComponent
    {
        private MyObjectBuilder_EntityBase builder;
        private Sandbox.ModAPI.IMyCargoContainer m_generator;
        private IMyCubeBlock m_parent;

        Sandbox.ModAPI.IMyTerminalBlock terminalBlock;

        private static ulong frameShift = 0;
        private ulong Frame;
        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            m_generator = Entity as Sandbox.ModAPI.IMyCargoContainer;
            m_parent = Entity as IMyCubeBlock;
            builder = objectBuilder;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME; //|= MyEntityUpdateEnum.EACH_FRAME

            terminalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }
        #region UpdateBeforeSimulation
        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();

            if (m_generator.IsWorking)
            {
                Frame = frameShift++;
                if (Frame % 30 != 0)
                {
                    return;
                }

                //IMyInventory inventory = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(0) as IMyInventory;

                //if (!inventory.ContainItems(10000, new MyObjectBuilder_Ingot { SubtypeName = "Coin" }))
                //{
                //    inventory.AddItems(5, new MyObjectBuilder_Ingot { SubtypeName = "Coin" });
                //    terminalBlock.RefreshCustomInfo();
                //}
                //IMyInventory inventory1 = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(1) as IMyInventory;
                //if (!inventory1.ContainItems(10, new MyObjectBuilder_AmmoMagazine { SubtypeName = "NATO_25x184mm" }))
                //{
                //    inventory1.AddItems(1, new MyObjectBuilder_AmmoMagazine { SubtypeName = "NATO_25x184mm" });
                //    terminalBlock.RefreshCustomInfo();
                //}


                List<IMyPlayer> players = new List<IMyPlayer>();
                MyAPIGateway.Players.GetPlayers(players, x => x.Controller != null && x.Controller.ControlledEntity != null);
                foreach (IMyPlayer player in players)
                {
                    if (player.IsBot)
                    {
                        continue;
                    }
                    if (player.Controller.ControlledEntity is IMyCharacter)
                    {

                        MyEntity entity = player.Controller.ControlledEntity.Entity as MyEntity;
                        if (entity.HasInventory)
                        {
                            IMyInventory inventory = entity.GetInventoryBase() as MyInventory;
                            if (!inventory.ContainItems(10000, new MyObjectBuilder_Ingot { SubtypeName = "Coin" }))
                            {
                                inventory.AddItems(60, new MyObjectBuilder_Ingot { SubtypeName = "Coin" });
                                terminalBlock.RefreshCustomInfo();
                            }
                        }
                    }

                }


            }
        }
        #endregion
        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return builder;
        }

    }

}
