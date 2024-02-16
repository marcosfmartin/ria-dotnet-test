using ATMCartridge.Models;

var inputAmounts = new List<int> { 30, 50, 60, 80, 140, 230, 370, 610, 980 };
var cartridges = new List<int> { 10, 50, 100 };

foreach (var inputAmount in inputAmounts)
{

    var possibleSolutions = new List<CartridgeSolution>();
    var possibleSolution = new CartridgeSolution();
    List<CartridgeFrequency> possible100 = GetPossibleNotesByAmount(inputAmount, 100);
    List<CartridgeFrequency> possible50 = GetPossibleNotesByAmount(inputAmount, 50);
    List<CartridgeFrequency> possible10 = GetPossibleNotesByAmount(inputAmount, 10);

    for (int i = 0; i < possible100.Count; i++)
    {
        var numberOfNotes = i + 1;
        var possibleFrequency100 = new CartridgeFrequency { CartridgeValue = 100, Frequency = numberOfNotes };

        var subset50 = GetSubsetOfOtherCartridge(numberOfNotes * 100, possible50, inputAmount);
        var restOfAmount = inputAmount;
        //get combinations with 50s
        foreach (var subsetElement in subset50)
        {
            possibleSolution = new CartridgeSolution();
            restOfAmount = AddNotesAndReduceAmountToSolution(subsetElement, inputAmount, possibleSolution);
            restOfAmount = AddNotesAndReduceAmountToSolution(possibleFrequency100, restOfAmount, possibleSolution);
            CompleteSolutionWith10s(restOfAmount, possibleSolution);
            possibleSolutions.Add(possibleSolution);
        }

        //get combination with 100 and 10s
        (possibleSolution, restOfAmount) = CreateSolutionWithInputFrequencyAndReduceAmount(possibleFrequency100, inputAmount);
        CompleteSolutionWith10s(restOfAmount, possibleSolution);
        possibleSolutions.Add(possibleSolution);
    }
    for (int i = 0; i < possible50.Count; i++)
    {
        var numberOfNotes = i + 1;
        var possibleFrequency50 = new CartridgeFrequency { CartridgeValue = 50, Frequency = numberOfNotes };
        possibleSolution = new CartridgeSolution();
        var restOfAmount = inputAmount;


        //get combination with 50 and 10s
        possibleSolution = new CartridgeSolution();
        restOfAmount = AddNotesAndReduceAmountToSolution(possibleFrequency50, restOfAmount, possibleSolution);
        CompleteSolutionWith10s(restOfAmount, possibleSolution);
        possibleSolutions.Add(possibleSolution);
    }

    possibleSolution = new CartridgeSolution();
    CompleteSolutionWith10s(inputAmount, possibleSolution);
    possibleSolutions.Add(possibleSolution);
    Console.WriteLine(@$"Possible solutions for {inputAmount}: ");
    PrintPossibleSolutions(possibleSolutions, inputAmount);
    Console.WriteLine();
}

void CompleteSolutionWith10s(int missingAmount, CartridgeSolution solution)
{
    if (missingAmount / 10 != 0) solution.PossibleCombination.Add(new CartridgeFrequency { CartridgeValue = 10, Frequency = missingAmount / 10 });
}

(CartridgeSolution, int) CreateSolutionWithInputFrequencyAndReduceAmount(CartridgeFrequency noteFrequency, int targetAmount)
{
    var possibleSolution = new CartridgeSolution();
    var remainingAmount = AddNotesAndReduceAmountToSolution(noteFrequency, targetAmount, possibleSolution);
    return (possibleSolution, remainingAmount);
}

int AddNotesAndReduceAmountToSolution(CartridgeFrequency noteFrequency, int targetAmount, CartridgeSolution solution)
{
    solution.PossibleCombination.Add(noteFrequency);
    var remainingAmount = targetAmount - (noteFrequency.Frequency * noteFrequency.CartridgeValue);
    return remainingAmount;
}

List<CartridgeFrequency> GetPossibleNotesByAmount(int amount, int note)
{
    var possibleNotes = new List<CartridgeFrequency>();
    for (int i = 1; i <= amount / note; i++)
    {
        if (note * i <= amount) possibleNotes.Add(new CartridgeFrequency { CartridgeValue = note, Frequency = i });
    }
    return possibleNotes;
}

List<CartridgeFrequency> GetSubsetOfOtherCartridge(int frequencySumValue, List<CartridgeFrequency> possibleFrequency, int targetAmount)
{
    var subset = new List<CartridgeFrequency>();
    foreach (var frequency in possibleFrequency)
    {
        if (frequencySumValue + (frequency.CartridgeValue * frequency.Frequency) <= targetAmount)
        {
            subset.Add(frequency);
        }
    }
    return subset;
}

void PrintPossibleSolutions(List<CartridgeSolution> cartridgeSolutions, int inputAmount)
{
    foreach (var solution in cartridgeSolutions)
    {
        for (int i = 0; i < solution.PossibleCombination.Count; i++)
        {
            var noteFrequency = solution.PossibleCombination[i];
            Console.Write($"{noteFrequency.Frequency} x {noteFrequency.CartridgeValue} EUR");
            if (i + 1 < solution.PossibleCombination.Count)
            {
                Console.Write($" + ");
            }
        }
        Console.WriteLine();
    }
}