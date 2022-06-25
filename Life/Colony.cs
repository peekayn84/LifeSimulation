using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class Colony
    {
        Random rnd = new Random();
        public int xCount;
        public int yCount;
        public List<People> people;
        public List<Food> foods;
        public List<House> houses;
        public List<Virus> viruses;
        public List<Virus> virusesToRemove;
        public List<People> peopleToRemove;
        struct NearObject{
            public int X { get; set; }
            public int Y { get; set; }
            public int DirectionX { get; set; }
            public int DirectionY { get; set; }
            public int Distance { get; set; }
            public int Type { get; set; }
            public NearObject(int x,int y, int directionX, int directionY, int distance, int type)
            {
                X = x;
                Y = y;
                DirectionX = directionX;
                DirectionY = directionY;
                Distance = distance;
                Type = type;
            }
        }
        
        public Colony(int xCount, int yCount)
        {
            this.xCount = xCount;
            this.yCount = yCount;
           people = new List<People>();
            foods = new List<Food>();
            houses = new List<House>();
            peopleToRemove = new List<People>();
            virusesToRemove = new List<Virus>();
            viruses = new List<Virus>();
            
        }
        public Colony(Colony other)
        {
            this.xCount = other.xCount;
            this.yCount = other.yCount;
            people = new List<People>(other.people);
            foods = new List<Food>(other.foods);
            houses = new List<House>(other.houses);
            peopleToRemove = new List<People>(other.peopleToRemove);
            virusesToRemove = new List<Virus>(other.virusesToRemove);
            viruses = new List<Virus>(other.viruses);

        }
        public void addPeople( int x, int y, int health, int feed, int old, int visualType, int virusStrength, bool virusGoDown, bool hasMask, int vaccineProtection, bool atHouse)
        {
            
            people.Add(new People( x,  y,  health,  feed,  old,  visualType,  virusStrength,  virusGoDown,  hasMask,  vaccineProtection,  atHouse));
        }
        public void addPeopleByCoord(int x, int y)
        {
            int visualType = rnd.Next(0, Settings.visualManCount);
            people.Add(new People(x, y, Settings.healthCellDefault, Settings.feedCellDefault, Settings.ageCellDefault, visualType, 0, false, false, 0, false));
        }
        public int countType(int type)
        {
            int result = 0;
            if (type == 8)
            {
                foreach (Virus virus in viruses)
                {
                    result++;
                }
            }
            else if (type == 4)
            {
                foreach (House house in houses)
                {

                    if (house.CurentCells < house.MaxCells)
                    {
                        result++;
                    }


                }
            }
            else if (type == 6)
            {
                foreach (House house in houses)
                {

                    if (house.CurentCells >= house.MaxCells)
                    {
                        result++;
                    }



                }
            }
            else if ((type == 7) || (type == 5) || (type == 1) || (type == -1))
            {
                foreach (People person in people)
                {
                    if (type == -1)
                    {
                        result++;
                    }


                    if (person.VirusStrength > 0)
                    {

                        if (person.HasMask)
                        {

                            if (type == 7)
                            {
                                result++;
                            }
                        }
                        else
                        {
                            if (type == 5)
                            {
                                result++;
                            }
                        }
                    }
                    else
                    {
                        if (type == 1)
                        {
                            result++;
                        }
                    }


                }
            }
            else if ((type == 3) || (type == 2))
            {
                foreach (Food food in foods)
                {

                    if (food.Vaccine)
                    {
                        if (type == 3)
                        {
                            result++;
                        }
                    }
                    else
                    {
                        if (type == 2)
                        {
                            result++;
                        }
                    }


                }
            }


            

            return result;
        }
        public void addPeopleRandom()
        {
            int x = 0;
            int y = 0;
            bool find = true;
            while (find)
            {
                x=rnd.Next(0,xCount);
                y=rnd.Next(0,yCount);
                find = false;
                foreach(Food food in foods)
                {

                    if ((food.X == x)&& (food.Y == y)){
                        find = true;
                        break;
                    }
                }
                foreach (House house in houses)
                {

                    if ((house.X == x) && (house.Y == y))
                    {
                        find = true;
                        break;
                    }
                }
            }
            int health = rnd.Next(1, Settings.healthCellDefault);
            int feed = rnd.Next(1, Settings.feedCellDefault);
            int old = rnd.Next(Settings.ageTillChild, Settings.ageAfterOld);
            int visualType = rnd.Next(0, Settings.visualManCount);
            bool hasMask = false;
            if (getRealRandomByPersent(50))
            {
                hasMask = true;
            }
            people.Add(new People( x, y, health, feed, old, visualType, 0, false, hasMask, 0, false));
        }
        public void addChildByCoord(int x,int y)
        {
 
            
            int visualType = rnd.Next(0, Settings.visualManCount);
            bool hasMask = false;
            if (getRealRandomByPersent(50))
            {
                hasMask = true;
            }
            people.Add(new People(x, y, Settings.healthCellDefault, Settings.feedCellDefault, 0, visualType, 0,false, hasMask, 0, true));
        }
        public void addVirusByCoord(int x, int y, int removeTime)
        {
            viruses.Add(new Virus(x, y, removeTime));
        }
        public void removeVirusByCoord(int x, int y)
        {
            for (int i = 0; i < viruses.Count; i++)
            {
                if ((viruses[i].X == x) && (viruses[i].Y == y))
                {
                    viruses.RemoveAt(i);
                    break;
                }
            }
        }
        public void removePersonByCoord(int x, int y)
        {
            for (int i = 0; i < people.Count; i++)
            {
                if ((people[i].X == x) && (people[i].Y == y))
                {
                    people.RemoveAt(i);
                    break;
                }
            }
        }
        public int getIdPersonByCoord(int x, int y)
        {
            for (int i = 0; i < people.Count; i++)
            {
                if ((people[i].X == x) && (people[i].Y == y))
                {
                    return i;
                }
            }
            return -1;
        }
        public void addFoodByCoord(int x, int y, bool vaccine)
        {
            int visualType = 0;
            if (!vaccine)
            {
                visualType = rnd.Next(0, Settings.visualFoodCount);
            }
            int tempFoodAdd = rnd.Next(Settings.minFoodAdd, Settings.maxFoodAdd + 1);
            foods.Add(new Food(x, y, tempFoodAdd, vaccine, visualType));
            
            
        }
        public void addHouseByCoord(int x, int y)
        {
            int visualType = rnd.Next(0, Settings.visualHouseCount);
            int sizeHouse=rnd.Next(Settings.minSizeHouse, Settings.maxSizeHouse+1);
            houses.Add(new House(x, y, visualType, sizeHouse, 0));


        }
        public void removeHouseByCoord(int x, int y)
        {
            for (int i = 0; i < houses.Count; i++)
            {
                if ((houses[i].X == x) && (houses[i].Y == y))
                {
                    houses.RemoveAt(i);
                    break;
                }
            }
        }
        public void changeCurentCellForHouse(int x, int y, int deltaCellCount)
        {
            Debug.WriteLine(deltaCellCount);
            for (int i = 0; i < houses.Count; i++)
            {
                if ((houses[i].X == x) && (houses[i].Y == y))
                {
                    houses[i].CurentCells = houses[i].CurentCells+deltaCellCount;
                    break;
                }
            }
        }
        public int countCellsReadyToGenerateNewLifeForHouse(int x, int y)
        {
            int count = 0;
            for (int i = 0; i < people.Count; i++)
            {
                if ((people[i].X == x) && (people[i].Y == y) && (people[i].AtHouse) && (people[i].Old<Settings.ageAfterOld) && (people[i].Old > Settings.ageTillChild))
                {
                    count++;
                }
            }
            return count;
        }
        public bool getRealRandomByPersent(int persent)
        {
            DateTime dt = DateTime.Now;
            int tempPersent = (int)(dt.Ticks % 101);
            //Debug.WriteLine(tempPersent.ToString() + " " + persent.ToString());
            if (tempPersent <= persent)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void removeFoodByCoord(int x, int y)
        {
            for(int i = 0; i < foods.Count; i++)
            {
                if ((foods[i].X == x) && (foods[i].Y == y))
                {
                    foods.RemoveAt(i);
                    break;
                }
            }
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
        public int getTypeByCoord(int x, int y)
        {
            foreach (Virus virus in viruses)
            {
                if ((virus.X == x) && (virus.Y == y))
                {
                    return 8;

                }
            }
            foreach (House house in houses)
            {
                if ((house.X == x) && (house.Y == y))
                {
                    if (house.CurentCells<house.MaxCells)
                    {
                        return 4;
                    }
                    else
                    {
                        return 6;
                    }

                }
            }
            foreach (People person in people)
            {
                if ((person.X == x) && (person.Y == y))
                {

                    if (person.VirusStrength > 0)
                    {

                        if (person.HasMask)
                        {

                            return 7;
                        }
                        else
                        {
                            return 5;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                    
                }
            }
            foreach (Food food in foods)
            {
                if ((food.X == x) && (food.Y == y))
                {
                    if (food.Vaccine)
                    {
                        return 3;
                    }
                    else
                    {
                        return 2;
                    }
                    
                }
            }
            return 0;
        }
        int debug = 0;
        bool moveToType(int idPerson, int type, List<NearObject> nearObjects)
        {
            bool moved = false;
            int idMinDistance = 0;
            int minDistance = Settings.radiusVision + 1;
            for (int i1 = 0; i1 < nearObjects.Count; i1++)
            {
                if ((nearObjects[i1].Distance < minDistance) && (nearObjects[i1].Type == type))
                {
                    idMinDistance = i1;
                    minDistance = nearObjects[i1].Distance;

                }

            }
            //Debug.WriteLine("Go to " + nearObjects[idMinDistance].DirectionX.ToString() + " " + nearObjects[idMinDistance].DirectionY.ToString());
            int newX = people[idPerson].X + nearObjects[idMinDistance].DirectionX;
            int newY = people[idPerson].Y + nearObjects[idMinDistance].DirectionY;
            int typeToGo = getTypeByCoord(newX, newY);
            if ((typeToGo != 1)&&(typeToGo != 5) && (typeToGo != 7))
            {

                if ((typeToGo == 4) && (type == 4))
                {
                    changeCurentCellForHouse(newX, newY,1);
                    people[idPerson].AtHouse = true;
                }
                else if ((typeToGo == 3) && (type == 3))
                {
                    people[idPerson].VaccineProtection = Settings.persentVaccineProtection;
                    removeFoodByCoord(newX, newY);
                }
                else if ((typeToGo == 2) && (type == 2))
                {
                    people[idPerson].Feed = Settings.feedCellDefault;
                    removeFoodByCoord(newX, newY);
                }
                else if (typeToGo == 8)
                {
                    removeVirusByCoord(newX, newY);
                    people[idPerson].VirusStrength=1;
                }
                people[idPerson].X = newX;
                people[idPerson].Y = newY;
                moved = true;
            }


            return moved;
        }
        bool moveRandom(int idPerson)
        {
            int counter = 0;
            while (true)
            {
                int newX = people[idPerson].X + (rnd.Next(3) - 1);
                int newY = people[idPerson].Y + (rnd.Next(3) - 1);
                int typeToGo = getTypeByCoord(newX, newY);
                if (((typeToGo == 0)|| (typeToGo == 8)) && (newX>0)&&(newX<xCount)&&(newY>0)&&(newY<yCount))
                {
                    if (typeToGo == 8)
                    {
                        removeVirusByCoord(newX, newY);
                        people[idPerson].VirusStrength = 1;
                    }
                    people[idPerson].X = newX;
                    people[idPerson].Y = newY;
                    return true;
                }
                counter++;
                if (counter == 20)
                {
                    return false;
                }
            }

            return false;
        }
        public Colony Clone()
        {
            return new Colony(this);
        }
        public void updateColony()
        {
            for (int i = 0; i < houses.Count; i++)
            {
                int coutTryToGenerateNewLife = countCellsReadyToGenerateNewLifeForHouse(houses[i].X, houses[i].Y) / Settings.countCellToChild;
                //Debug.WriteLine("q"+ coutTryToGenerateNewLife.ToString());
                if (coutTryToGenerateNewLife >= 1)
                {
                    while (coutTryToGenerateNewLife != 0)
                    {
                        coutTryToGenerateNewLife = coutTryToGenerateNewLife - 1;
                        if ((getRealRandomByPersent(Settings.percentChild))&& (houses[i].CurentCells < houses[i].MaxCells))
                        {
                            Debug.WriteLine("Add child");
                            addChildByCoord(houses[i].X, houses[i].Y);
                            houses[i].CurentCells++;
                        }
                    }
                }
            }
            for(int i = 0; i < people.Count(); i++)
            {
                if (people[i].Old > Settings.ageTillChild)
                {
                    if (people[i].AtHouse)
                    {
                        int persentToGoToFood = (int)(100 - (people[i].Feed * 1.0 / Settings.feedCellDefault * 100));

                        //Debug.WriteLine(persentToGoToFood);
                        if (getRealRandomByPersent(persentToGoToFood))
                        {
                            int xHouse = people[i].X;
                            int yHouse = people[i].Y;
                            bool exited = moveRandom(i);
                            if (exited)
                            {
                                people[i].AtHouse = false;
                                changeCurentCellForHouse(xHouse, yHouse, -1);
                            }
                        }

                    }
                    else
                    {
                        if (getRealRandomByPersent(Settings.movePercent))
                        {

                            bool moved = false;
                            if (Settings.radiusVision > 0)
                            {
                                List<NearObject> nearObjects = new List<NearObject>();
                                bool findFood = false;
                                bool findVaccine = false;
                                bool findHouse = false;
                                int minXVision = 0;
                                if (people[i].X - Settings.radiusVision > 0)
                                {
                                    minXVision = people[i].X - Settings.radiusVision;
                                }
                                int minYVision = 0;
                                if (people[i].Y - Settings.radiusVision > 0)
                                {
                                    minYVision = people[i].Y - Settings.radiusVision;
                                }
                                int maxXVision = xCount;
                                if (people[i].X + Settings.radiusVision < xCount)
                                {
                                    maxXVision = people[i].X + Settings.radiusVision + 1;
                                }
                                int maxYVision = yCount;
                                if (people[i].Y - Settings.radiusVision > 0)
                                {
                                    maxYVision = people[i].Y + Settings.radiusVision + 1;
                                }


                                for (int xTemp = minXVision; xTemp < maxXVision; xTemp++)
                                {
                                    for (int yTemp = minYVision; yTemp < maxYVision; yTemp++)
                                    {
                                        if (!((xTemp == people[i].X) && (yTemp == people[i].Y)))
                                        {
                                            int type = getTypeByCoord(xTemp, yTemp);
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
                                                NearObject nearObject = new NearObject();
                                                nearObject.X = xTemp;
                                                nearObject.Y = yTemp;
                                                nearObject.Type = type;
                                                nearObject.DirectionX = 0;
                                                if (xTemp < people[i].X)
                                                {
                                                    nearObject.DirectionX = -1;
                                                }
                                                else if (xTemp > people[i].X)
                                                {
                                                    nearObject.DirectionX = 1;
                                                }
                                                nearObject.DirectionY = 0;
                                                if (yTemp < people[i].Y)
                                                {
                                                    nearObject.DirectionY = -1;
                                                }
                                                else if (yTemp > people[i].Y)
                                                {
                                                    nearObject.DirectionY = 1;
                                                }
                                                int distanceX = Math.Abs(xTemp - people[i].X);
                                                int distanceY = Math.Abs(yTemp - people[i].Y);
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
                                int persentToGoToFood = (int)(100 - (people[i].Feed * 1.0 / Settings.feedCellDefault * 100));
                                if (findFood)
                                {
                                    //Debug.WriteLine(persentToGoToFood);
                                    if (getRealRandomByPersent(persentToGoToFood))
                                    {
                                        //Debug.WriteLine(debug.ToString() + "Go to food");
                                        //debug++;
                                        moved = moveToType(i, 2, nearObjects);
                                    }
                                }
                                if ((!moved) && (findVaccine))
                                {
                                    int persentToGoToVaccine = (int)(100 - (people[i].VaccineProtection * 1.0 / Settings.persentVaccineProtection * 100));
                                    if (getRealRandomByPersent(persentToGoToVaccine))
                                    {
                                        moved = moveToType(i, 3, nearObjects);
                                    }
                                }
                                if ((!moved) && (findHouse))
                                {
                                    if (getRealRandomByPersent(100 - persentToGoToFood))
                                    {
                                        moved = moveToType(i, 4, nearObjects);
                                    }
                                }
                                

                            }
                            if (!moved)
                            {
                                moveRandom(i);
                            }
                            int persentToInfected = 0;
                            for(int xTemp = people[i].X-1; xTemp < people[i].X+1; xTemp++)
                            {
                                for (int yTemp = people[i].Y - 1; yTemp < people[i].Y + 1; yTemp++)
                                {
                                    if ((!((xTemp == people[i].X) && (yTemp == people[i].Y)))&&(xTemp>0)&&(xTemp<xCount) && (yTemp > 0) && (yTemp < yCount))
                                    {
                                        int typeNeighbor=getTypeByCoord(xTemp, yTemp);
                                        if ((typeNeighbor == 5) || (typeNeighbor == 7))
                                        {
                                            int tempInfected = 0;
                                            if (typeNeighbor == 5)
                                            {
                                                tempInfected += Settings.percentToInfectedNeighbor;
                                            }else if(typeNeighbor == 7)
                                            {
                                                tempInfected += Settings.percentToInfectedNeighbor-Settings.percentMaskInfectedWeak;
                                            }
                                            if (people[i].HasMask)
                                            {
                                                tempInfected = tempInfected - Settings.percentMaskHealthyWeak;
                                            }
                                            if (people[i].VaccineProtection>0)
                                            {
                                                tempInfected = tempInfected - people[i].VaccineProtection;
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
                                if (getRealRandomByPersent(persentToInfected))
                                {
                                    people[i].VirusStrength = 1;
                                }
                            }

                        }

                    }
                    if (people[i].Feed > 0)
                    {
                        people[i].Feed--;
                    }
                    else
                    {
                        people[i].Health-=3;
                    }
                    if (people[i].VaccineProtection > 0)
                    {
                        people[i].VaccineProtection = people[i].VaccineProtection-Settings.protectionVaccineGoDown;
                        if (people[i].VaccineProtection < 0)
                        {
                            people[i].VaccineProtection = 0;
                        }
                    }
                    if (people[i].VirusStrength > 0)
                    {
                        people[i].Health-= people[i].VirusStrength;
                        if (people[i].VirusGoDown)
                        {
                            people[i].VirusStrength--;
                            if (people[i].VirusStrength == 0)
                            {
                                people[i].VirusGoDown = false;
                            }
                        }
                        else
                        {
                            if (getRealRandomByPersent(Settings.percentVirusGoDown))
                            {
                                people[i].VirusGoDown = true;
                            }
                            people[i].VirusStrength++;
                        }

                    }
                    else
                    {
                        if (getRealRandomByPersent(Settings.percentToInfectedFromAir))
                        {
                            people[i].VirusStrength++;
                        }
                    }
                    if ((people[i].Feed > (Settings.feedCellDefault / 2))&& (people[i].VirusStrength==0) && (people[i].Old < Settings.ageAfterOld) && (people[i].Health < Settings.healthCellDefault))
                    {
                        people[i].Health++;
                    }
                    if (people[i].Health <= 0)
                    {
                        if (people[i].VirusStrength > 0)
                        {
                            if (getRealRandomByPersent(Settings.percentToStayVirusAfterDie))
                            {
                                if ((getTypeByCoord(people[i].X, people[i].Y) != 4)&& (getTypeByCoord(people[i].X, people[i].Y) != 6))
                                {
                                    addVirusByCoord(people[i].X, people[i].Y, Settings.virusCellRemoveAfterDie + 1);
                                }
                                
                            }

                            
                        }
                        peopleToRemove.Add(people[i]);

                    }
                    if (people[i].Old > Settings.ageAfterOld)
                    {
                        int persentToDie =(int)((people[i].Old - Settings.ageAfterOld)*1.0 / (Settings.maxAge - Settings.ageAfterOld )*100);
                        if (getRealRandomByPersent(persentToDie))
                        {
                            peopleToRemove.Add(people[i]);
                        }
                    }

                }
                people[i].Old++;




            }
            if (peopleToRemove.Count > 0)
            {
                for (int i = 0; i < peopleToRemove.Count; i++)
                {
                    if (peopleToRemove[i].AtHouse)
                    {

                        changeCurentCellForHouse(people[i].X, people[i].Y, -1);
                    }
                    people.Remove(peopleToRemove[i]);

                }
                peopleToRemove.Clear();
            }
            for (int i = 0; i < viruses.Count; i++)
            {
                viruses[i].Health--;
                if (viruses[i].Health == 0)
                {
                    virusesToRemove.Add(viruses[i]);
                }
            }
            if (virusesToRemove.Count > 0)
            {
                for (int i = 0; i < virusesToRemove.Count; i++)
                {
                    viruses.Remove(virusesToRemove[i]);
                }
                virusesToRemove.Clear();
            }
            int vaccineOnMap = 0;
            int foodOnMap = 0;
            for(int i = 0; i < foods.Count; i++)
            {
                if (foods[i].Vaccine)
                {
                    vaccineOnMap++;
                }
                else
                {
                    foodOnMap++;
                }
            }
            if (Settings.maxFoodOnMap > foodOnMap)
            {
                if (getRealRandomByPersent(Settings.persentToGenerateFood))
                {
                    int xTemp=rnd.Next(0, xCount);
                    int yTemp=rnd.Next(0, yCount);
                    int foodAdd=rnd.Next(Settings.minFoodAdd, Settings.maxFoodAdd);
                    int visualType = rnd.Next(0, Settings.visualFoodCount);
                    if (getTypeByCoord(xTemp, yTemp) == 0)
                    {
                        foods.Add(new Food(xTemp, yTemp, foodAdd, false, visualType));
                    }
                }
            }
            if (Settings.maxVaccineOnMap > vaccineOnMap)
            {
                if (getRealRandomByPersent(Settings.persentToGenerateVaccine))
                {
                    int xTemp = rnd.Next(0, xCount);
                    int yTemp = rnd.Next(0, yCount);
                    int foodAdd = rnd.Next(Settings.minFoodAdd, Settings.maxFoodAdd);
                    int visualType = rnd.Next(0, Settings.visualFoodCount);
                    if (getTypeByCoord(xTemp, yTemp) == 0)
                    {
                        foods.Add(new Food(xTemp, yTemp, foodAdd, true, visualType));
                    }
                }
            }

        }
    }
}
