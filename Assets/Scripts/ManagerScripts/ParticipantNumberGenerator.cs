using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ParticipantNumberGenerator : MonoBehaviour
{
    public string FilePath; 
    
    public static ParticipantNumberGenerator Instance { get; private set; } // used to allow easy access of this script in other scripts

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that this instance is the only one and is accessible globally
        if (Instance == null)
        {
            Instance = this;
        }

        FilePath = Path.Combine(Application.dataPath, "participants.txt");


    }
    
    public int GenerateUniqueParticipantNumber()
    {
        HashSet<int> existingNumbers = LoadExistingNumbers();
        
        int newNumber;
        do
        {
            newNumber = UnityEngine.Random.Range(1000, 10000); // Example range
        } while (existingNumbers.Contains(newNumber));
        
        SaveParticipantNumber(newNumber);
        
        return newNumber;
    }

    private HashSet<int> LoadExistingNumbers()
    {
        HashSet<int> existingNumbers = new HashSet<int>();

        if (File.Exists(FilePath))
        {
            string[] lines = File.ReadAllLines(FilePath);
            foreach (string line in lines)
            {
                if (int.TryParse(line, out int number))
                {
                    existingNumbers.Add(number);
                }
            }
        }

        return existingNumbers;
    }

    private void SaveParticipantNumber(int number)
    {
        using (StreamWriter writer = new StreamWriter(FilePath, true))
        {
            writer.WriteLine(number);
        }
    }
}
