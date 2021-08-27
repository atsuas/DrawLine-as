using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject LinePrefabs;
    //レイヤーを定義
    public LayerMask CantDrawOverLayer;
    int CantDrawOverLayerIndex;


    public float LinePointsMinDistance;
    public float LineWidth;
    public Gradient LineColor;


    Line currentLine;


    private void Start()
    {
        //CantDrawOverという名前のものをCantDrawOverLayerIndexに入れる
        CantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginDraw();
        }

        //LineがNullだったらDraw関数を実行
        if (currentLine != null)
        {
            Draw();
        }

        //ボタンが上がりきったらEndDraw関数を実行
        if (Input.GetMouseButtonUp(0))
        {
            EndDraw();
        }
    }

    //書き始めの処理
    private void BeginDraw()
    {
        //Line.csからトランスフォームを取ってきて、Lineプレハブで書く
        currentLine = Instantiate(LinePrefabs, this.transform).GetComponent<Line>();

        currentLine.SetLineColor(LineColor);
        currentLine.SetLineWidth(LineWidth);
        currentLine.SetPointMinDistance(LinePointsMinDistance);
        //最初はPhysicsは消しておくから落ちない
        currentLine.UsePhysics(false);
    }

    private void Draw()
    {
        //マウスポジション取得するための定番のやつ
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //こんな感じで設定できる　→　Physics2D.CircleCast (Vector2 origin, float radius, Vector2 direction, float distance= Mathf.Infinity, int layerMask= DefaultRaycastLayers, float minDepth= -Mathf.Infinity, float maxDepth= Mathf.Infinity);
        RaycastHit2D hit = Physics2D.CircleCast(MousePos, LineWidth / 3f, Vector2.zero, 5f, CantDrawOverLayer); ;

        if (hit)
            EndDraw();
        else
            currentLine.AddPoint(MousePos);
    }

    private void EndDraw()
    {
        if (currentLine != null)
        {
            if (currentLine.pointsCount < 2)
            {
                Destroy(currentLine.gameObject);
            }
            else
            {
                currentLine.gameObject.layer = CantDrawOverLayerIndex;
                //trueにして、Physics取得するから落ちる
                currentLine.UsePhysics(true);
                currentLine = null;
            }
        }
    }

}
