using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    static class Settings
    {
        //Visual
        public static int sizeBar = 4;
        public static int barPadding = 5;
        public static int sizeCell = 100;
        public static int visualManCount = 3;
        public static int visualHouseCount = 2;
        public static int visualFoodCount = 1;
        public static int visualVaccineCount = 1;
        //Size map
        public static int xCount = 15;
        public static int yCount = 10;
        
        //Cell
        public static int movePercent = 100; //Движеться на одну клетку
        public static int feedCellDefault = 15; //Стандарт = Максимум, сытость > 50% = регенерация, сытость < 0 потеря здоровья
        public static int healthCellDefault = 50;//Стандарт = Максимум
        public static int ageCellDefault = 0;//Начальное состояние возраста
        public static int countCellToChild = 2;//Размер "пар"
        public static int percentChild = 45;//Вероятность создания ребенка
        public static int ageTillChild = 16;//Возвраст совершенолетия
        public static int ageAfterOld = 80;//
        public static int maxAge = 100;
        public static int radiusVision = 3;
        //House
        public static int minSizeHouse = 10;//Мин размер по людям в доме
        public static int maxSizeHouse = 20;//Макс размер по людям в доме
        //Создать дома(с параметрами) и клетки(с параметрами)
        //Virus
        public static int percentToInfectedFromAir = 1;//Шанс заразиться по воздуху, без зараженных
        public static int percentToInfectedNeighbor = 50;//Если два соседа то шанс 100%, шанс заразить сумируеться
        public static int percentMaskInfectedWeak = 25;
        public static int percentMaskHealthyWeak = 15;
        public static int percentVirusGoDown = 10;//Расчет шанса пойти на поправку, обновляеться каждый ход
        public static int percentToStayVirusAfterDie = 50;
        public static int virusCellRemoveAfterDie = 3;
        //Vaccine
        public static int persentVaccineProtection = 70;//Шанс вакцины спасти от заражения
        public static int protectionVaccineGoDown = 3;//Снижение эфективности факцины, можно предусмотреть нелинейность
        public static int persentToGenerateVaccine = 10;
        public static int maxVaccineOnMap = 5;
        //Food
        public static int minFoodAdd = 10;
        public static int maxFoodAdd = 15;
        public static int persentToGenerateFood = 50;
        public static int maxFoodOnMap = 20;

        

    }
}
