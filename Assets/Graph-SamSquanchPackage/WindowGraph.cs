using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SamSquanchLibrary.Functions;
using HelperLibrary.UI;
using CT.Lookup;

public class WindowGraph : MonoBehaviour
{
   private static WindowGraph instance;
   [SerializeField] private Sprite dotSprite;
   private RectTransform graphContainer;
   private RectTransform labelTemplateX;
   private RectTransform labelTemplateY;

   private RectTransform dashTemplateX;
   private RectTransform dashTemplateY;
   

   private List<GameObject> gameObjectList;
   private List<IGraphVisualObject> graphVisualObjectList;

   private GameObject tooltipGameObject;
   
   private IGraphVisual graphVisual;
   private int maxVisibleValueAmount; 
   private Func<int, string> getAxisLabelX;
   private Func<float, string> getAxisLabelY;

   //Example data set
   List<float> valueList;

   private bool isLineChartActive = true;


    //Example graph
    //IGraphVisual lineGraphVisual;
    IGraphVisual popLineGraphVisual;
    IGraphVisual moneyLineGraphVisual;
    IGraphVisual foodLineGraphVisual;
    IGraphVisual scienceLineGraphVisual;


    private void Awake()
   {
       instance = this;
       //Set up references 
       graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
       //labelTemplateX = graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
       //labelTemplateY = graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
       dashTemplateX = graphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
       dashTemplateY = graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();
      // tooltipGameObject = graphContainer.Find("GraphToolTip").gameObject;

       gameObjectList = new List<GameObject>();
       graphVisualObjectList = new List<IGraphVisualObject>();

        //Intitialise different chart visuals (For multiple Displays) 

        //Population graph
        popLineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.white, Color.white);
       
       //Money graph
       moneyLineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, DataSheet.WORKER_COLOUR, DataSheet.WORKER_COLOUR);

       //Food Graph
       foodLineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, DataSheet.FARMER_COLOUR, DataSheet.FARMER_COLOUR);

       //Science Graph
       scienceLineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, DataSheet.SCIENTIST_COLOUR, DataSheet.SCIENTIST_COLOUR);

        isLineChartActive = true;
    }

    public void UpdateAndShowGraphs(List<float> moneyValues, List<float> scienceValues, List<float> foodValues, List<float> popValues)
    {
        //Clear previous list of values before displaying new ones
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }

        gameObjectList.Clear();


        foreach (IGraphVisualObject graphVisualObject in graphVisualObjectList)
        {
            graphVisualObject.CleanUp();
        }

        graphVisualObjectList.Clear();

        popLineGraphVisual.CleanUp();
        moneyLineGraphVisual.CleanUp();
        foodLineGraphVisual.CleanUp();
        scienceLineGraphVisual.CleanUp();

        List<float> biggestList = RAUtility.GetListWithMaxValue(moneyValues, scienceValues, foodValues, popValues);
        biggestList.Add(0);

        SetGraphVisual(moneyLineGraphVisual, moneyValues, biggestList);
        SetGraphVisual(scienceLineGraphVisual, scienceValues, biggestList);
        SetGraphVisual(foodLineGraphVisual, foodValues, biggestList);
        SetGraphVisual(popLineGraphVisual, popValues, biggestList);
    }

  public static void HideToolTip_Static()
  {
   // instance.HideToolTip();
  }

  private void SetGraphVisual(IGraphVisual graphVisual, List<float> _valueList, List<float> _scaleList)
  {
      ShowGraph(_valueList, _scaleList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
  }

   private void ShowGraph(List<float> valueList, List<float> scaleList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
   {
      //Cache values whenever this function is called
      this.valueList = valueList;
      this.graphVisual = graphVisual;
      this.getAxisLabelX = getAxisLabelX;
      this.getAxisLabelY = getAxisLabelY;

      
     
      //Set visible count to value list size if 0
      if(maxVisibleValueAmount <= 0)
      {
        maxVisibleValueAmount = valueList.Count;
      }
      
      if(maxVisibleValueAmount > valueList.Count)
      {
        maxVisibleValueAmount = valueList.Count;
      }

      this.maxVisibleValueAmount = maxVisibleValueAmount; //This is here as it makes it easier to change
      
      if(getAxisLabelX == null)
      {
        getAxisLabelX = delegate (int _i) {return _i.ToString(); }; //Default behavior delegated to set x label
      }
      if(getAxisLabelY == null)
      {
        getAxisLabelY = delegate (float _f) {return _f.ToString(); }; //Default behavior delegated to set Y label
      }

      float graphHeight = graphContainer.sizeDelta.y;
      float graphWidth = graphContainer.sizeDelta.x;
      
      float yMaximum = scaleList[0]; //Would be used for what data you want to show value for e.g. carbon captured
      float yMinimum = scaleList[0]; //Setting minimum and max values based off first element of values list

      //Set Y graph values dynamically 
      for (int i = Mathf.Max(scaleList.Count - maxVisibleValueAmount, 0); i < scaleList.Count; i++)
      {
        float value = scaleList[i];
        if(value > yMaximum)
        {
            yMaximum = value;
        }

        if(value < yMinimum)
        {
            yMinimum = value;
        }
      }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f); //Set max size with 20 percent leway
        yMinimum = yMinimum - (yDifference * 0.2f); //Set min size with 20 percent leway

        float xSize = graphWidth / (maxVisibleValueAmount + 1); //Would be used for time frame e.g. year or month
      int xIndex = 0;
      
      for(int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
      {
        float xPosition = xSize + xIndex * xSize;
            float yPosition = (((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight);
          
        //Set data point
        string tooltipText = getAxisLabelY(valueList[i]);
        graphVisualObjectList.Add(graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), xSize, tooltipText));
       
        RectTransform dashY = Instantiate(dashTemplateY);
        dashY.SetParent(graphContainer, false);
        dashY.gameObject.SetActive(true);
        dashY.anchoredPosition = new Vector2(xPosition, 0f);
        gameObjectList.Add(dashY.gameObject);

        xIndex++;
        


      }

      int serpratorCount = 10; //The number of labels for y axis graph values
      for(int i = 0; i <= serpratorCount; i++)
      {
       float normalizedValue = i * 1f / serpratorCount; 
        RectTransform dashX = Instantiate(dashTemplateX);
        dashX.SetParent(graphContainer, false);
        dashX.gameObject.SetActive(true);
        dashX.anchoredPosition = new Vector2(0f, normalizedValue * graphHeight);
        gameObjectList.Add(dashX.gameObject);
      }
   }

   
private interface IGraphVisual 
{
  IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
  void CleanUp();
}


//A single visual object in the graph
private interface IGraphVisualObject
{
   void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string HideToolTip);
   void CleanUp();
}   
   

   private class LineGraphVisual : IGraphVisual
   {
      private RectTransform graphContainer;
      private Sprite dotSprite;
      private LineGraphVisualObject lastLineGraphVisualObject;
      //private GameObject lastDotGameObject;
      private Color dotColor;
      private Color dotConnectionColor;
     
      //Set up required references for this class in the constructor
      public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor)
      {
        this.graphContainer = graphContainer;
        this.dotSprite = dotSprite;
        this.dotColor = dotColor;
        this.dotConnectionColor = dotConnectionColor;
        lastLineGraphVisualObject = null;
      }

      public void CleanUp() {
            lastLineGraphVisualObject = null;
        }

      public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
      {
        List<GameObject> gameObjectList = new List<GameObject>();
        GameObject dotGameObject = CreateDot(graphPosition);
        
        
  
        
        gameObjectList.Add(dotGameObject); //Create new list of values to display
        GameObject dotConnectionGameObject = null;
        
        //Check if circle has been placed previously and draw line to new point
        if(lastLineGraphVisualObject != null)
        {
            dotConnectionGameObject = CreateDotConnection(lastLineGraphVisualObject.GetGraphPosition(), dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            gameObjectList.Add(dotConnectionGameObject);
            
        }

        

        LineGraphVisualObject lineGraphVisualObject = new LineGraphVisualObject(dotGameObject, dotConnectionGameObject, lastLineGraphVisualObject);
        
        lineGraphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
        
        lastLineGraphVisualObject = lineGraphVisualObject;

        return lineGraphVisualObject;
      }

      private GameObject CreateDot(Vector2 anchoredPosition)
      {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = dotSprite;
        gameObject.GetComponent<Image>().color = dotColor;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        gameObject.transform.SetAsLastSibling();
         //Show tooltip of value when mouse hovers over a bar 
        Button_UI dotButtonUI = gameObject.AddComponent<Button_UI>();

        return gameObject;
      }

      private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
      {
         GameObject gameObject = new GameObject("dotConnection", typeof(Image));
         gameObject.transform.SetParent(graphContainer, false);
         gameObject.GetComponent<Image>().color = dotConnectionColor;
         RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
         Vector2 dir = (dotPositionB - dotPositionA).normalized;
         float distance = Vector2.Distance(dotPositionA, dotPositionB);
         rectTransform.anchorMin = new Vector2(0, 0);
         rectTransform.anchorMax = new Vector2(0, 0);
         rectTransform.sizeDelta = new Vector2(distance, 3f);
         rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
         rectTransform.localEulerAngles = new Vector3(0, 0, HelperFunctions.GetAngleFromVectorFloat(dir)); //Rotate line towards direction

         return gameObject;
      }

      public class LineGraphVisualObject : IGraphVisualObject
      {
          public event EventHandler OnChangedGraphVisualObjectInfo;
          private GameObject dotGameObject; 
          private GameObject dotConnectionGameObject;
          private LineGraphVisualObject lastVisualObject;

        public LineGraphVisualObject(GameObject dotGameObject, GameObject dotConnectionGameObject, LineGraphVisualObject lastVisualObject)
        {
          
           this.dotGameObject = dotGameObject;
           this.dotConnectionGameObject = dotConnectionGameObject;
           this.lastVisualObject = lastVisualObject;
           
           
           if(lastVisualObject != null)
           {
              lastVisualObject.OnChangedGraphVisualObjectInfo += LastVisualObject_OnChangedGraphVisualObjectInfo;
           }
          
        }
        
        private void LastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e)
        {
           UpdateDotConnection();
        }
        public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
        {
           RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
           rectTransform.anchoredPosition = graphPosition;

          
           UpdateDotConnection();
           
           Button_UI dotButtonUI = dotGameObject.GetComponent<Button_UI>();

           if(OnChangedGraphVisualObjectInfo != null) OnChangedGraphVisualObjectInfo(this, EventArgs.Empty);
           
              
          
        }
        public void CleanUp()
        {
          Destroy(dotGameObject);
          Destroy(dotConnectionGameObject);
        }

        public Vector2 GetGraphPosition()
        {
          RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
          
          return rectTransform.anchoredPosition;
        }

        private void UpdateDotConnection()
        {
           if(dotConnectionGameObject != null)
           {
              RectTransform dotConnectionRectTransform = dotConnectionGameObject.GetComponent<RectTransform>();
              Vector2 dir = (lastVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
              float distance = Vector2.Distance(GetGraphPosition(), lastVisualObject.GetGraphPosition());
              dotConnectionRectTransform.sizeDelta = new Vector2(distance, 3f);
              dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + dir * distance * .5f;
              dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, HelperFunctions.GetAngleFromVectorFloat(dir));

           }
        }

      }
   }
}
