using System;

namespace RuterKodetest
{
    class Program
    {
        static void Main(string[] args)
        {
            var callGenerator = new PersonCallGenerator();
            var elevator = new Elevator();

            while (true)
            {
                callGenerator.CreatePersonCall(elevator);

                callGenerator.CreatePersonCall(elevator);
            }
        }
    }

    interface IElevator
    {
        void GotoFloor(int destinationFloor);
        void LoadPeople(Person[] people);
        void TransportToFloor(int floor);
    }

    class Elevator : IElevator
    {
        private readonly int _maximumTotalWeight;
        private readonly int[] _floors;
        private int _currentFloor;
        private Person[] _peopleInElevator;

        public Elevator()
        {
            _currentFloor = 0;
            _floors = new int[8];
            _maximumTotalWeight = 600;
        }

        public void GotoFloor(int destinationFloor)
        {
            if (destinationFloor > (_floors.Length - 1) && destinationFloor > 0)
                throw new Exception("Invalid floor.");

            if (destinationFloor == _currentFloor) return;

            _currentFloor = destinationFloor;
        }

        public void LoadPeople(Person[] people)
        {
            _peopleInElevator = people;

            int currentTotalWeight = 0;
            foreach (var person in people)
                currentTotalWeight += person.Weight;

            if (currentTotalWeight > _maximumTotalWeight)
                throw new Exception("Total weight exceeded.");
        }

        public void TransportToFloor(int floor)
        {
            if (_peopleInElevator.Length == 0 || _peopleInElevator is null)
                throw new Exception("Invalid operation: Elevator is empty on transport.");

            GotoFloor(floor);

            _peopleInElevator = null;
        }
    }

    class Person
    {
        public int Weight { get; set; }
        public Person(int weight)
        {
            Weight = weight;
        }
    }

    class PersonFactory
    {
        private static readonly Random _numberGenerator = new Random(); 

        public Person CreatePerson()
        {
            int weight = _numberGenerator.Next(0, 150);
            return new Person(weight);
        }
    }

    class PersonCallGenerator
    {
        private readonly PersonFactory _personFactory;

        public PersonCallGenerator()
        {
            _personFactory = new PersonFactory(); //should be dependency injection irl
        }

        public void CreatePersonCall(Elevator elevator)
        {
            Console.Write("Enter call floor: ");
            var callFloor = Convert.ToInt32(Console.ReadLine());

            elevator.GotoFloor(callFloor);

            Console.Write("Enter number of people: ");
            var numberOfPeople = Convert.ToInt32(Console.ReadLine());

            var people = new Person[numberOfPeople];

            for (int i = 0; i < numberOfPeople; i++)
                people[i] = _personFactory.CreatePerson();

            elevator.LoadPeople(people);
        }

        public void CreatePersonDestination(Elevator elevator)
        {
            var destinationFloor = Convert.ToInt32(Console.ReadLine());

            elevator.TransportToFloor(destinationFloor);
        }
    }
}
