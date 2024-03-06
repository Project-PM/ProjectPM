using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPickerElement
{
    public string name;
    public double weigted;

    public RandomPickerElement(string name, double weigted)
    {
        this.name = name;
        this.weigted = weigted;
    }
}

public class WeightedRandomPicker : MonoBehaviour
{
    public static string GetRandomPicker(List<RandomPickerElement> list)
    {
        System.Random randomInstance = new System.Random();

        Dictionary<string, double> itemWeightDict = new Dictionary<string, double>();
        double sumOfWeights = 0; // 모든 가중치 합

        foreach (var item in list)
        {
            if (itemWeightDict.ContainsKey(item.name))
            {
                Debug.LogError("중복 아이템 존재");
                continue;
            }

            sumOfWeights += item.weigted;
            itemWeightDict.Add(item.name, item.weigted);
        }

        double randomValue = randomInstance.NextDouble(); // 0.0 ~ 1.0 사이 값 추출
        randomValue *= sumOfWeights;

        double current = 0.0;
        foreach(var pair in itemWeightDict)
        {
            current += pair.Value;

            if (randomValue <= current)
                return pair.Key;
        }

        Debug.LogWarning("랜덤 뽑기에 실패");
        return null;
    }
}
