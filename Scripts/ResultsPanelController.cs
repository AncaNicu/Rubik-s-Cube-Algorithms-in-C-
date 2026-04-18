using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsPanelController : MonoBehaviour
{
    public TextMeshProUGUI resultsText;

    public void Start()
    {
        resultsText.text = "Shuffle the cube and choose an algorithm to solve it.";
    }

    public void ShowStartingMessage(string algorithmName)
    {
        resultsText.text = $"<b>Running {algorithmName} algorithm...</b>";
    }

    public void ShowSuccessResult(string algorithmName, List<string> steps, int moveCount, float time)
    {
        resultsText.text =
            $"<b>Results of {algorithmName}</b>\n\n" +
            $"<b>Solution found!</b>\n" +
            $"<b>Solution:</b> {string.Join(" ", steps)}\n" +
            $"<b>Solution length:</b> {moveCount}\n" +
            $"<b>Time:</b> {GetTimeString(time)}";
    }

    public void ShowFailureResult(string algorithmName, float time)
    {
        resultsText.text =
            $"<b>Results of {algorithmName}</b>\n\n" +
            $"<b>No solution found!</b>\n" +
            $"<b>Time:</b> {GetTimeString(time)}";
    }

    public void ShowCubeAlreadySolvedeResult(string algorithmName, float time)
    {
        resultsText.text =
            $"<b>Results of {algorithmName}</b>\n\n" +
            $"<b>Cube was already solved!</b>\n" +
            $"<b>Time:</b> {GetTimeString(time)}";
    }

    //fct care transforma timpul in cea mai potrivita unitate de masura
    //si returneaza valoarea timpului urmata de unitate
    private string GetTimeString(float time)
    {
        //timp de ordinul minutelor
        if(time > 60000)
        {
            time /= 60000f;
            return (time.ToString("F3") + " min");
        }

        //timp de ordinul secundelor
        else if(time > 1000)
        {
            time /= 1000f;
            return (time.ToString("F3") + " s");
        }

        //timp de ordinul ms
        return (time.ToString("F3") + " ms");
    }
}
