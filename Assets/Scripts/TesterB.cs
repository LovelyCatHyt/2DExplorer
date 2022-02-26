using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TesterB : MonoBehaviour
{
    [Inject] private Tester _tester;

    private void Start()
    {
        Debug.Log(_tester);
    }
}
