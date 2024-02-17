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

    #region Get All possible combinations involving at least one 100EUR
    for (int i = 0; i < possible100.Count; i++)
    {
        var numberOfNotes = i + 1;
        var possibleFrequency100 = new CartridgeFrequency { CartridgeValue = 100, Frequency = numberOfNotes };

        var subset50 = GetPossibleSubsetsOfSmallerCartridge(numberOfNotes * 100, possible50, inputAmount);
        var remainingAmount = inputAmount;
        //get combinations with 50s
        foreach (var subsetElement in subset50)
        {
            possibleSolution = new CartridgeSolution();
            remainingAmount = AddNotesAndReduceAmountToSolution(subsetElement, inputAmount, possibleSolution);
            remainingAmount = AddNotesAndReduceAmountToSolution(possibleFrequency100, remainingAmount, possibleSolution);
            CompleteSolutionWith10s(remainingAmount, possibleSolution);
            possibleSolutions.Add(possibleSolution);
        }

        //get combination with 100 and 10s
        possibleSolution = new CartridgeSolution();
        remainingAmount = inputAmount;
        remainingAmount = AddNotesAndReduceAmountToSolution(possibleFrequency100, remainingAmount, possibleSolution);
        CompleteSolutionWith10s(remainingAmount, possibleSolution);
        possibleSolutions.Add(possibleSolution);
    }
    #endregion

    #region Get all combinations involving at least one 50 EUR, but no 100 EUR
    for (int i = 0; i < possible50.Count; i++)
    {
        var numberOfNotes = i + 1;
        var possibleFrequency50 = new CartridgeFrequency { CartridgeValue = 50, Frequency = numberOfNotes };
        possibleSolution = new CartridgeSolution();
        var remainingAmount = inputAmount;

        possibleSolution = new CartridgeSolution();
        remainingAmount = AddNotesAndReduceAmountToSolution(possibleFrequency50, remainingAmount, possibleSolution);
        CompleteSolutionWith10s(remainingAmount, possibleSolution);
        possibleSolutions.Add(possibleSolution);
    }
    #endregion

    #region Get the one solution which only have 10s
    possibleSolution = new CartridgeSolution();
    CompleteSolutionWith10s(inputAmount, possibleSolution);
    possibleSolutions.Add(possibleSolution);
    #endregion


    Console.WriteLine(@$"Possible solutions for {inputAmount}: ");
    PrintPossibleSolutions(possibleSolutions, inputAmount);
    Console.WriteLine();
}

void CompleteSolutionWith10s(int missingAmount, CartridgeSolution solution)
{
    if (missingAmount / 10 != 0) solution.PossibleCombination.Add(new CartridgeFrequency { CartridgeValue = 10, Frequency = missingAmount / 10 });
}

int AddNotesAndReduceAmountToSolution(CartridgeFrequency noteFrequency, int targetAmount, CartridgeSolution solution)
{
    solution.PossibleCombination.Add(noteFrequency);
    var remainingAmount = targetAmount - (noteFrequency.Frequency * noteFrequency.CartridgeValue);
    return remainingAmount;
}

List<CartridgeFrequency> GetPossibleNotesByAmount(int amount, int note)
{
    //gets possible number of cartridges for a given amount. For example, if the amount is 400 and we are talking about 100 cartridge, we could have at most 4. So resulting array would be [1, 2, 3, 4]
    var possibleNotes = new List<CartridgeFrequency>();
    for (int i = 1; i <= amount / note; i++)
    {
        if (note * i <= amount) possibleNotes.Add(new CartridgeFrequency { CartridgeValue = note, Frequency = i });
    }
    return possibleNotes;
}

List<CartridgeFrequency> GetPossibleSubsetsOfSmallerCartridge(int biggerCartidgeValue, List<CartridgeFrequency> possibleFrequency, int targetAmount)
{
    //Given a array like the one described in the above method, gets a subset of this array that makes sense for the amount. Example:
    //Amount is 300. We have [1,2,3] for 100 cartridge and [1,2,3,4,5,6] for 50 cartridge.
    //If we are talking about the 2 * 100 case, the possible subset for 50 cartidge is [1,2]. The others would exceed the amount.
    var subset = new List<CartridgeFrequency>();
    foreach (var frequency in possibleFrequency)
    {
        if (biggerCartidgeValue + (frequency.CartridgeValue * frequency.Frequency) <= targetAmount)
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