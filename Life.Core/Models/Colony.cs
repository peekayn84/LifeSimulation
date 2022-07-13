using Life.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Life.Core.Models
{
    public sealed class Colony
    {
        private readonly Random _random;
        private readonly Config _config;

        // private int _debug = 0;

        public int ColumnsCount { get; }
        public int RowsCount { get; }

        public List<Person> People { get; }
        public List<Food> FoodItems { get; }
        public List<House> Houses { get; }
        public List<Virus> Viruses { get; }
        public List<Virus> VirusesToRemove { get; }
        public List<Person> PeopleToRemove { get; }

        struct NearbyObject
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int DirectionX { get; set; }
            public int DirectionY { get; set; }
            public int Distance { get; set; }
            public int Type { get; set; }

            public NearbyObject(int x, int y, int directionX, int directionY, int distance, int type)
            {
                X = x;
                Y = y;
                DirectionX = directionX;
                DirectionY = directionY;
                Distance = distance;
                Type = type;
            }
        }

        public Colony(int columnsCount, int rowsCount, IConfigManager<Config> configManager)
        {
            if(configManager.Configuration == null)
            {
                throw new Exception("Configuration was null.");
            }

            _random = new Random();
            _config = configManager.Configuration;

            ColumnsCount = columnsCount;
            RowsCount = rowsCount;
            People = new List<Person>();
            FoodItems = new List<Food>();
            Houses = new List<House>();
            PeopleToRemove = new List<Person>();
            VirusesToRemove = new List<Virus>();
            Viruses = new List<Virus>();
        }

        public Colony(Colony other)
        {
            _random = new Random();
            _config = other._config;

            ColumnsCount = other.ColumnsCount;
            RowsCount = other.RowsCount;
            People = new List<Person>(other.People);
            FoodItems = new List<Food>(other.FoodItems);
            Houses = new List<House>(other.Houses);
            PeopleToRemove = new List<Person>(other.PeopleToRemove);
            VirusesToRemove = new List<Virus>(other.VirusesToRemove);
            Viruses = new List<Virus>(other.Viruses);
        }

        public void AddPerson(
            int x,
            int y,
            int health,
            int saturation,
            int age,
            int visualType,
            int virusStrength,
            bool virusGoDown,
            bool hasMask,
            int vaccineProtection,
            bool atHouse)
        {
            People.Add(new Person(x, y, health, saturation, age, visualType, virusStrength, virusGoDown, hasMask, vaccineProtection, atHouse));
        }

        public void AddPeopleByCoord(int x, int y)
        {
            int visualType = _random.Next(0, AssetsSettings.PersonIconsCount);
            AddPerson(
                x,
                y,
                _config.PersonDefaultHealth,
                _config.PersonDefaultSaturation,
                _config.PersonDefaultAge,
                visualType,
                virusStrength: 0,
                virusGoDown: false,
                hasMask: false,
                vaccineProtection: 0,
                atHouse: false
            );
        }

        public int CountType(int type) => type switch
        {
            8 => Viruses.Count,
            4 => Houses.Count((h) => h.CurrentCapacity < h.MaxCapacity),
            6 => Houses.Count((h) => h.CurrentCapacity >= h.MaxCapacity),
            3 => FoodItems.Count((f) => f.IsVaccine),
            2 => FoodItems.Count((f) => !f.IsVaccine),
            -1 => People.Count,
            7 => People.Count((p) => p.VirusStrength > 0 && p.HasMask),
            5 => People.Count((p) => p.VirusStrength > 0 && !p.HasMask),
            1 => People.Count((p) => p.VirusStrength <= 0),
            _ => throw new Exception($"Invalid type value: {type}")
        };

        public void RandomlyAddHouse()
        {
            int x = _random.Next(0, ColumnsCount);
            int y = _random.Next(0, RowsCount);

            AddHouseByCoord(x, y);
        }

        public void RandomlyAddPerson()
        {
            int x = 0;
            int y = 0;
            bool find = true;

            while (find)
            {
                x = _random.Next(0, ColumnsCount);
                y = _random.Next(0, RowsCount);
                find = false;

                foreach (Food food in FoodItems)
                {
                    if (IsInTheCell(food, x, y))
                    {
                        find = true;
                        break;
                    }
                }

                foreach (House house in Houses)
                {
                    if (IsInTheCell(house, x, y))
                    {
                        find = true;
                        break;
                    }
                }
            }

            int health = _random.Next(1, _config.PersonDefaultHealth);
            int saturation = _random.Next(1, _config.PersonDefaultSaturation);
            int age = _random.Next(_config.AdultAge, _config.OldAge);
            int visualType = _random.Next(0, AssetsSettings.PersonIconsCount);
            bool hasMask = GetRealRandomByPersent(50);

            AddPerson(
                x,
                y,
                health,
                saturation,
                age,
                visualType,
                virusStrength: 0,
                virusGoDown: false,
                hasMask,
                vaccineProtection: 0,
                atHouse: false
            );
        }

        public void AddChildByCoord(int x, int y)
        {
            int visualType = _random.Next(0, AssetsSettings.PersonIconsCount);
            bool hasMask = GetRealRandomByPersent(50);

            AddPerson(
               x,
               y,
               _config.PersonDefaultHealth,
               _config.PersonDefaultSaturation,
               age: 0,
               visualType,
               virusStrength: 0,
               virusGoDown: false,
               hasMask: false,
               vaccineProtection: 0,
               atHouse: false
           );
        }

        public void AddVirusByCoord(int x, int y, int health)
        {
            Viruses.Add(new Virus(x, y, health));
        }

        public void RemoveVirusByCoord(int x, int y)
        {
            var target = Viruses.FirstOrDefault((v) => IsInTheCell(v, x, y));
            if (target != null)
            {
                Viruses.Remove(target);
            }
        }

        public void RemoveFoodByCoord(int x, int y)
        {
            var target = FoodItems.FirstOrDefault((f) => IsInTheCell(f, x, y));
            if (target != null)
            {
                FoodItems.Remove(target);
            }
        }

        public void RemovePersonByCoord(int x, int y)
        {
            var target = People.FirstOrDefault((p) => IsInTheCell(p, x, y));
            if (target != null)
            {
                People.Remove(target);
            }
        }

        public void RemoveHouseByCoord(int x, int y)
        {
            var target = Houses.FirstOrDefault((h) => IsInTheCell(h, x, y));
            if (target != null)
            {
                Houses.Remove(target);
            }
        }


        public int GetPersonIdxByCoord(int x, int y)
        {
            return People.FindIndex((p) => IsInTheCell(p, x, y));
        }

        public void AddFoodByCoord(int x, int y, bool vaccine)
        {
            int visualType = 0;
            if (!vaccine)
            {
                visualType = _random.Next(0, AssetsSettings.FoodIconsCount);
            }
            int tempFoodAdd = _random.Next(_config.MinFoodIncrement, _config.MaxFoodIncrement + 1);           
            FoodItems.Add(new Food(x, y, tempFoodAdd, vaccine, visualType));
        }

        public void AddHouseByCoord(int x, int y)
        {
            int visualType = _random.Next(0, AssetsSettings.HouseIconsCount);
            int sizeHouse = _random.Next(_config.MinHouseCapacity, _config.MaxHouseCapacity + 1);
            Houses.Add(new House(x, y, visualType, sizeHouse, 0));
        }

        public void ChangeCurentCellForHouse(int x, int y, int deltaCellCount)
        {
            Debug.WriteLine(deltaCellCount);
            var target = Houses.FirstOrDefault((h) => IsInTheCell(h, x, y));
            if (target != null)
            {
                target.CurrentCapacity += deltaCellCount;
            }
        }

        public int CountCellsReadyToGenerateNewLifeForHouse(int x, int y)
            => People.Count((p) => IsPersonReadyToGenerateNewLife(p, x, y));

        private bool IsPersonReadyToGenerateNewLife(Person p, int x, int y)
            => p.X == x
            && p.Y == y
            && p.AtHouse
            && p.Age < _config.OldAge
            && p.Age > _config.AdultAge;

        private static bool IsInTheCell(CellBase cell, int x, int y)
            => cell.X == x && cell.Y == y;

        public bool GetRealRandomByPersent(int percent)
        {
            DateTime dt = DateTime.Now;
            int tempPersent = (int)(dt.Ticks % 101);
            //Debug.WriteLine(tempPersent.ToString() + " " + persent.ToString());
            return tempPersent <= percent;
        }

        //0-Nothing
        //1-Person
        //2-Food
        //3-Vaccine
        //4-Free House
        //5-Infected Person
        //6-Full House
        //7-Infected person with mask
        //8-Virus
        public int GetTypeByCoord(int x, int y)
        {
            if (Viruses.Any((v) => IsInTheCell(v, x, y))) return 8;
            if (Houses.Any((h) => IsInTheCell(h, x, y) && h.CurrentCapacity < h.MaxCapacity)) return 4;
            if (Houses.Any((h) => IsInTheCell(h, x, y) && h.CurrentCapacity >= h.MaxCapacity)) return 6;
            if (People.Any((p) => IsInTheCell(p, x, y) && p.VirusStrength > 0 && p.HasMask)) return 7;
            if (People.Any((p) => IsInTheCell(p, x, y) && p.VirusStrength > 0 && !p.HasMask)) return 5;
            if (People.Any((p) => IsInTheCell(p, x, y) && p.VirusStrength <= 0)) return 1;
            if (FoodItems.Any((f) => IsInTheCell(f, x, y) && f.IsVaccine)) return 3;
            if (FoodItems.Any((f) => IsInTheCell(f, x, y))) return 2;

            return 0;
        }

        bool MoveToType(int personId, int type, List<NearbyObject> nearbyObjects)
        {
            bool moved = false;
            int idMinDistance = 0;
            int minDistance = _config.VisionRadius + 1;

            for (int i1 = 0; i1 < nearbyObjects.Count; i1++)
            {
                if ((nearbyObjects[i1].Distance < minDistance) && (nearbyObjects[i1].Type == type))
                {
                    idMinDistance = i1;
                    minDistance = nearbyObjects[i1].Distance;
                }
            }

            //Debug.WriteLine("Go to " + nearObjects[idMinDistance].DirectionX.ToString() + " " + nearObjects[idMinDistance].DirectionY.ToString());
            int newX = People[personId].X + nearbyObjects[idMinDistance].DirectionX;
            int newY = People[personId].Y + nearbyObjects[idMinDistance].DirectionY;
            int typeToGo = GetTypeByCoord(newX, newY);
            if ((typeToGo != 1) && (typeToGo != 5) && (typeToGo != 7))
            {

                if ((typeToGo == 4) && (type == 4))
                {
                    ChangeCurentCellForHouse(newX, newY, 1);
                    People[personId].AtHouse = true;
                }
                else if ((typeToGo == 3) && (type == 3))
                {
                    People[personId].VaccineProtection = _config.InitialVaccinePower;
                    RemoveFoodByCoord(newX, newY);
                }
                else if ((typeToGo == 2) && (type == 2))
                {
                    People[personId].Saturation = _config.PersonDefaultSaturation;
                    RemoveFoodByCoord(newX, newY);
                }
                else if (typeToGo == 8)
                {
                    RemoveVirusByCoord(newX, newY);
                    People[personId].VirusStrength = 1;
                }

                People[personId].X = newX;
                People[personId].Y = newY;
                moved = true;
            }

            return moved;
        }

        bool MoveRandom(int idPerson)
        {
            int counter = 0;
            while (true)
            {
                int newX = People[idPerson].X + (_random.Next(3) - 1);
                int newY = People[idPerson].Y + (_random.Next(3) - 1);
                int typeToGo = GetTypeByCoord(newX, newY);
                if (((typeToGo == 0) || (typeToGo == 8)) && (newX > 0) && (newX < ColumnsCount) && (newY > 0) && (newY < RowsCount))
                {
                    if (typeToGo == 8)
                    {
                        RemoveVirusByCoord(newX, newY);
                        People[idPerson].VirusStrength = 1;
                    }
                    People[idPerson].X = newX;
                    People[idPerson].Y = newY;
                    return true;
                }
                counter++;
                if (counter == 20)
                {
                    return false;
                }
            }
        }

        public Colony Clone()
        {
            return new Colony(this);
        }

        public void UpdateColony()
        {
            for (int i = 0; i < Houses.Count; i++)
            {
                int coutTryToGenerateNewLife = CountCellsReadyToGenerateNewLifeForHouse(Houses[i].X, Houses[i].Y) / _config.PeopleToCreatePersonCount;
                //Debug.WriteLine("q"+ coutTryToGenerateNewLife.ToString());
                if (coutTryToGenerateNewLife >= 1)
                {
                    while (coutTryToGenerateNewLife != 0)
                    {
                        coutTryToGenerateNewLife = coutTryToGenerateNewLife - 1;
                        if ((GetRealRandomByPersent(_config.PersonCreateChance)) && (Houses[i].CurrentCapacity < Houses[i].MaxCapacity))
                        {
                            Debug.WriteLine("Add child");
                            AddChildByCoord(Houses[i].X, Houses[i].Y);
                            Houses[i].CurrentCapacity++;
                        }
                    }
                }
            }

            for (int i = 0; i < People.Count(); i++)
            {
                People[i].PrevX = -1;
                People[i].PrevY = -1;
                if (People[i].Age > _config.AdultAge)
                {
                    if (People[i].AtHouse)
                    {
                        int persentToGoToFood = (int)(100 - (People[i].Saturation * 1.0 / _config.PersonDefaultSaturation * 100));

                        
                        if (GetRealRandomByPersent(persentToGoToFood))
                        {
                            int xHouse = People[i].X;
                            int yHouse = People[i].Y;
                            bool exited = MoveRandom(i);

                            if (exited)
                            {
                                People[i].AtHouse = false;
                                ChangeCurentCellForHouse(xHouse, yHouse, -1);
                            }
                        }

                    }
                    else
                    {
                        if (GetRealRandomByPersent(_config.PersonMoveChance))
                        {

                            bool moved = false;
                            if (_config.VisionRadius > 0)
                            {
                                List<NearbyObject> nearObjects = new List<NearbyObject>();
                                bool findFood = false;
                                bool findVaccine = false;
                                bool findHouse = false;
                                int minXVision = 0;

                                if (People[i].X - _config.VisionRadius > 0)
                                {
                                    minXVision = People[i].X - _config.VisionRadius;
                                }

                                int minYVision = 0;
                                if (People[i].Y - _config.VisionRadius > 0)
                                {
                                    minYVision = People[i].Y - _config.VisionRadius;
                                }

                                int maxXVision = ColumnsCount;
                                if (People[i].X + _config.VisionRadius < ColumnsCount)
                                {
                                    maxXVision = People[i].X + _config.VisionRadius + 1;
                                }

                                int maxYVision = RowsCount;
                                if (People[i].Y - _config.VisionRadius > 0)
                                {
                                    maxYVision = People[i].Y + _config.VisionRadius + 1;
                                }


                                for (int xTemp = minXVision; xTemp < maxXVision; xTemp++)
                                {
                                    for (int yTemp = minYVision; yTemp < maxYVision; yTemp++)
                                    {
                                        if (!((xTemp == People[i].X) && (yTemp == People[i].Y)))
                                        {
                                            int type = GetTypeByCoord(xTemp, yTemp);
                                            if (type != 0)
                                            {
                                                if (type == 2)
                                                {
                                                    findFood = true;
                                                }
                                                else if (type == 3)
                                                {
                                                    findVaccine = true;
                                                }
                                                else if (type == 4)
                                                {
                                                    findHouse = true;
                                                }

                                                NearbyObject nearObject = new NearbyObject();
                                                nearObject.X = xTemp;
                                                nearObject.Y = yTemp;
                                                nearObject.Type = type;
                                                nearObject.DirectionX = 0;

                                                if (xTemp < People[i].X)
                                                {
                                                    nearObject.DirectionX = -1;
                                                }
                                                else if (xTemp > People[i].X)
                                                {
                                                    nearObject.DirectionX = 1;
                                                }

                                                nearObject.DirectionY = 0;
                                                if (yTemp < People[i].Y)
                                                {
                                                    nearObject.DirectionY = -1;
                                                }
                                                else if (yTemp > People[i].Y)
                                                {
                                                    nearObject.DirectionY = 1;
                                                }

                                                int distanceX = Math.Abs(xTemp - People[i].X);
                                                int distanceY = Math.Abs(yTemp - People[i].Y);
                                                if (distanceX > distanceY)
                                                {
                                                    nearObject.Distance = distanceX;
                                                }
                                                else if (distanceX < distanceY)
                                                {
                                                    nearObject.Distance = distanceY;
                                                }
                                                else
                                                {
                                                    nearObject.Distance = distanceX;
                                                }
                                                nearObjects.Add(nearObject);
                                                //Debug.WriteLine("Find Type=" + nearObject.Type.ToString() + " X=" + nearObject.DirectionX.ToString() + " Y=" + nearObject.DirectionY.ToString() + " Dist=" + nearObject.Distance.ToString());
                                            }
                                        }
                                    }
                                }

                                int persentToGoToFood = (int)(100 - (People[i].Saturation * 1.0 / _config.PersonDefaultSaturation * 100));
                                if (findFood)
                                {
                                    //Debug.WriteLine(persentToGoToFood);
                                    if (GetRealRandomByPersent(persentToGoToFood))
                                    {
                                        //Debug.WriteLine(debug.ToString() + "Go to food");
                                        //debug++;
                                        moved = MoveToType(i, 2, nearObjects);
                                    }
                                }

                                if ((!moved) && (findVaccine))
                                {
                                    int persentToGoToVaccine = (int)(100 - (People[i].VaccineProtection * 1.0 / _config.InitialVaccinePower * 100));
                                    if (GetRealRandomByPersent(persentToGoToVaccine))
                                    {
                                        moved = MoveToType(i, 3, nearObjects);
                                    }
                                }

                                if ((!moved) && (findHouse))
                                {
                                    if (GetRealRandomByPersent(100 - persentToGoToFood))
                                    {
                                        moved = MoveToType(i, 4, nearObjects);
                                    }
                                }
                            }

                            if (!moved)
                            {
                                MoveRandom(i);
                            }

                            int persentToInfected = 0;
                            for (int xTemp = People[i].X - 1; xTemp < People[i].X + 1; xTemp++)
                            {
                                for (int yTemp = People[i].Y - 1; yTemp < People[i].Y + 1; yTemp++)
                                {
                                    if ((!((xTemp == People[i].X) && (yTemp == People[i].Y))) && (xTemp > 0) && (xTemp < ColumnsCount) && (yTemp > 0) && (yTemp < RowsCount))
                                    {
                                        int typeNeighbor = GetTypeByCoord(xTemp, yTemp);
                                        if ((typeNeighbor == 5) || (typeNeighbor == 7))
                                        {
                                            int tempInfected = 0;
                                            if (typeNeighbor == 5)
                                            {
                                                tempInfected += _config.ChanceToInfectNeighbour;
                                            }
                                            else if (typeNeighbor == 7)
                                            {
                                                tempInfected += _config.ChanceToInfectNeighbour - _config.ChanceToInfectWhenInMask;
                                            }

                                            if (People[i].HasMask)
                                            {
                                                tempInfected = tempInfected - _config.ChanceToBeInfectedWhenInMask;
                                            }

                                            if (People[i].VaccineProtection > 0)
                                            {
                                                tempInfected = tempInfected - People[i].VaccineProtection;
                                            }

                                            if (tempInfected > 0)
                                            {
                                                persentToInfected += tempInfected;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persentToInfected > 0)
                            {
                                if (GetRealRandomByPersent(persentToInfected))
                                {
                                    People[i].VirusStrength = 1;
                                }
                            }

                        }

                    }

                    if (People[i].Saturation > 0)
                    {
                        People[i].Saturation--;
                    }
                    else
                    {
                        People[i].Health -= 3;
                    }

                    if (People[i].VaccineProtection > 0)
                    {
                        People[i].VaccineProtection = People[i].VaccineProtection - _config.VaccinePowerDecrement;
                        if (People[i].VaccineProtection < 0)
                        {
                            People[i].VaccineProtection = 0;
                        }
                    }

                    if (People[i].VirusStrength > 0)
                    {
                        People[i].Health -= People[i].VirusStrength;

                        if (People[i].VirusGoDown)
                        {
                            People[i].VirusStrength--;

                            if (People[i].VirusStrength == 0)
                            {
                                People[i].VirusGoDown = false;
                            }
                        }
                        else
                        {
                            if (GetRealRandomByPersent(_config.InitialChanceVirusGoDown))
                            {
                                People[i].VirusGoDown = true;
                            }

                            People[i].VirusStrength++;
                        }

                    }
                    else
                    {
                        if (GetRealRandomByPersent(_config.ChanceToGetInfectedWithAir))
                        {
                            People[i].VirusStrength++;
                        }
                    }

                    if ((People[i].Saturation > (_config.PersonDefaultSaturation / 2)) && (People[i].VirusStrength == 0) && (People[i].Age < _config.OldAge) && (People[i].Health < _config.PersonDefaultHealth))
                    {
                        People[i].Health++;
                    }

                    if (People[i].Health <= 0)
                    {
                        if (People[i].VirusStrength > 0)
                        {
                            if (GetRealRandomByPersent(_config.ChanceToCreateVirusOnDeath))
                            {
                                if ((GetTypeByCoord(People[i].X, People[i].Y) != 4) && (GetTypeByCoord(People[i].X, People[i].Y) != 6))
                                {
                                    AddVirusByCoord(People[i].X, People[i].Y, _config.MovesToVirusDeath + 1);
                                }
                            }
                        }
                        PeopleToRemove.Add(People[i]);
                    }

                    if (People[i].Age > _config.OldAge)
                    {
                        int persentToDie = (int)((People[i].Age - _config.OldAge) * 1.0 / (_config.MaxAge - _config.OldAge) * 100);

                        if (GetRealRandomByPersent(persentToDie))
                        {
                            PeopleToRemove.Add(People[i]);
                        }
                    }

                }

                People[i].Age++;
            }

            if (PeopleToRemove.Count > 0)
            {
                for (int i = 0; i < PeopleToRemove.Count; i++)
                {
                    if (PeopleToRemove[i].AtHouse)
                    {
                        ChangeCurentCellForHouse(People[i].X, People[i].Y, -1);
                    }

                    People.Remove(PeopleToRemove[i]);
                }

                PeopleToRemove.Clear();
            }

            for (int i = 0; i < Viruses.Count; i++)
            {
                Viruses[i].Health--;

                if (Viruses[i].Health == 0)
                {
                    VirusesToRemove.Add(Viruses[i]);
                }
            }

            if (VirusesToRemove.Count > 0)
            {
                for (int i = 0; i < VirusesToRemove.Count; i++)
                {
                    Viruses.Remove(VirusesToRemove[i]);
                }

                VirusesToRemove.Clear();
            }

            int vaccineOnMap = 0;
            int foodOnMap = 0;

            for (int i = 0; i < FoodItems.Count; i++)
            {
                if (FoodItems[i].IsVaccine)
                {
                    vaccineOnMap++;
                }
                else
                {
                    foodOnMap++;
                }
            }

            if (_config.MaxFoodItemsOnMap > foodOnMap)
            {
                if (GetRealRandomByPersent(_config.ChanceToGenerateFood))
                {
                    int xTemp = _random.Next(0, ColumnsCount);
                    int yTemp = _random.Next(0, RowsCount);
                    int foodAdd = _random.Next(_config.MinFoodIncrement, _config.MaxFoodIncrement);
                    int visualType = _random.Next(0, AssetsSettings.FoodIconsCount);

                    if (GetTypeByCoord(xTemp, yTemp) == 0)
                    {
                        FoodItems.Add(new Food(xTemp, yTemp, foodAdd, false, visualType));
                    }
                }
            }

            if (_config.MaxVaccineItemsOnField > vaccineOnMap)
            {
                if (GetRealRandomByPersent(_config.ChanceToGenerateVaccine))
                {
                    int xTemp = _random.Next(0, ColumnsCount);
                    int yTemp = _random.Next(0, RowsCount);
                    int foodAdd = _random.Next(_config.MinFoodIncrement, _config.MaxFoodIncrement);
                    int visualType = _random.Next(0, AssetsSettings.VaccineIconsCount);

                    if (GetTypeByCoord(xTemp, yTemp) == 0)
                    {
                        FoodItems.Add(new Food(xTemp, yTemp, foodAdd, true, visualType));
                    }
                }
            }

        }
    }
}
