using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SamSquanchLibrary.Functions;
using HelperLibrary.UI;

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

   //private bool isBarChartActive = false;
   private bool isLineChartActive = true;


    //Example graph
    //IGraphVisual lineGraphVisual;
    IGraphVisual popLineGraphVisual;
    IGraphVisual moneyLineGraphVisual;
    IGraphVisual foodLineGraphVisual;
    IGraphVisual scienceLineGraphVisual;
    //IGraphVisual barChartVisual;


    private void Awake()
   {
       instance = this;
       //Set up references 
       graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
       labelTemplateX = graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
       labelTemplateY = graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
       dashTemplateX = graphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
       dashTemplateY = graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();
       tooltipGameObject = graphContainer.Find("GraphToolTip").gameObject;

       gameObjectList = new List<GameObject>();
       graphVisualObjectList = new List<IGraphVisualObject>();

       //Example data set used for all buttons, as in my GRD project data sets were being handled outside of this script. All graph visual data set are set to this dummy list
       //valueList = new List<float>() {0, 5000, 900, 8000, 600, 7000, 10000};



        //Intitialise different chart visuals (For multiple Displays) 

        //Population graph
        popLineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, new Color(1, 1, 1, .5f));
       //barChartVisual = new BarChartVisual(graphContainer, Color.red, .8f);
       
       //Money graph
       moneyLineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, new Color(238, 130, 238, .3f));
       //IGraphVisual moneyBarChartVisual = new BarChartVisual(graphContainer, Color.green, .8f);

       //Food Graph
       foodLineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.white, new Color(255, 0, 0, .3f));
       //IGraphVisual co2BarChartVisual = new BarChartVisual(graphContainer, Color.red, .8f);

       //Science Graph
       scienceLineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.blue, new Color(176, 224, 230, .3f));
        //IGraphVisual treesBarChartVisual = new BarChartVisual(graphContainer, Color.blue, .8f);







        //ShowGraph(valueList, barChartVisual, -1, (int _i) => "Day " + (_i + 1), (float _f) => "$" + _f);

        //Graph functionality button listner and delegate set up
        /*
        transform.Find("BarChartButton").GetComponent<Button_UI>().ClickFunc = () => 
        {
          isBarChartActive = true;
          isLineChartActive = false;
          SetGraphVisual(barChartVisual);
        };
        */

        isLineChartActive = true;
        UpdateAndShowGraphs(
            new List<float>() { 500, 2555, 900, 8000, 600, 7000, 2334 },
            new List<float>() { 1000, 5000, 2000, 8000, 600, 7000, 8665 },
            new List<float>() { 1000, 3222, 4268, 5253, 612, 2689, 5378 },
            new List<float>() { 1000, 8421, 8537, 8000, 600, 7000, 10000 }
            );

        /*
       transform.Find("LineGraphButton").GetComponent<Button_UI>().ClickFunc = () => 
       {
         //isBarChartActive = false;
         isLineChartActive = true;
         SetGraphVisual(lineGraphVisual);
       };
       */

        /*
        transform.Find("DecreaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = () =>
       {
          DecreaseVisibleAmount();
       };

       transform.Find("IncreaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = () =>
       {
          IncreaseVisibleAmount();
       };
       */

        /*
       //Value set toggle buttons listener and delegate set up
        transform.Find("MoneyValueListToggleBtn").GetComponent<Button_UI>().ClickFunc = () =>
       {
           if(isLineChartActive && valueList[0] != null)
           {
              ShowGraph(valueList, moneyLineGraphVisual, -1, (int _i) => "Year " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f) + "M");
              lineGraphVisual = moneyLineGraphVisual;
           }

           else if (isBarChartActive && valueList[0] != null)
           {
              ShowGraph(valueList, moneyBarChartVisual, -1, (int _i) => "Year " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f) + "M");
              barChartVisual = moneyBarChartVisual;
           }
       };

        transform.Find("GlobalCo2ValueListToggleBtn").GetComponent<Button_UI>().ClickFunc = () =>
       {
           if(isLineChartActive && valueList[0] != null)
           {
              ShowGraph(valueList, co2LineGraphVisual, -1, (int _i) => "Year " + (_i + 1), (float _f) => "°C" + _f);
              lineGraphVisual = co2LineGraphVisual;
           }
           else if (isBarChartActive && valueList[0] != null)
           {
              ShowGraph(valueList, co2BarChartVisual, -1, (int _i) => "Year " + (_i + 1), (float _f) => "°C" + _f);
              barChartVisual = co2BarChartVisual;
           }
       };

        transform.Find("TreesPlantedValueListToggleBtn").GetComponent<Button_UI>().ClickFunc = () =>
       {
            if(isLineChartActive && valueList[0] != null)
           {
              ShowGraph(valueList, treesLineGraphVisual, -1, (int _i) => "Year " + (_i + 1), (float _f) => "Trees" + Mathf.RoundToInt(_f));
              lineGraphVisual = treesLineGraphVisual;
           }
           else if (isBarChartActive && valueList[0] != null)
           {
              ShowGraph(valueList, treesBarChartVisual, -1, (int _i) => "Year " + (_i + 1), (float _f) => "Trees" + Mathf.RoundToInt(_f));
              barChartVisual = treesBarChartVisual;
           }
       };
       */


        /////////TESTING PURPOSES WILL CREATE RANDOM CHART VALUES EVERY .5 OF A SECOND TO SHOW DYNAMIC GRAPH AT WORK////////////
        /*
         FunctionPeriodic.Create(() => {
              valueList.Clear();
              for(int i = 0; i < UnityEngine.Random.Range(5, 25); i++)
              {
                  valueList.Add(UnityEngine.Random.Range(0,500));
              }

              ShowGraph(valueList, graphVisual, -1, (int _i) => "Year: " + (_i + 10), (float _f) => Mathf.RoundToInt(_f) + "Tons");
         }, .5f);  
         */

    }
  
  public static void ShowToolTip_Static(string tooltipText, Vector2 anchoredPosition)
  {
     instance.ShowToolTip(tooltipText, anchoredPosition);
  }
  private void ShowToolTip(string tooltipText, Vector2 anchoredPosition)
  {
    tooltipGameObject.SetActive(true);
    
    tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    Text tooltipUIText = tooltipGameObject.transform.Find("Text").GetComponent<Text>();
    tooltipUIText.text = tooltipText;

    float textPaddingSize = 4f;

    Vector2 backgroundSize = new Vector2(tooltipUIText.preferredWidth + textPaddingSize * 2f, tooltipUIText.preferredHeight + textPaddingSize * 2f);

    tooltipGameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = backgroundSize;
    
    tooltipGameObject.transform.SetAsLastSibling(); //So it shows up on top of graph
  }

    public void UpdateAndShowGraphs(List<float> popValues, List<float> moneyValues, List<float> foodValues, List<float> scienceValues)
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

        SetGraphVisual(popLineGraphVisual, popValues);
        SetGraphVisual(moneyLineGraphVisual, moneyValues);
        SetGraphVisual(foodLineGraphVisual, foodValues);
        SetGraphVisual(scienceLineGraphVisual, scienceValues);
    }

  public static void HideToolTip_Static()
  {
    instance.HideToolTip();
  }

  private void HideToolTip()
  {
    tooltipGameObject.SetActive(false);
  }

  private void SetAxisLabelX(Func<int, string> getAxisLabelX)
  {
      ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, getAxisLabelX, this.getAxisLabelY);
  }
  
  private void SetAxisLabelY(Func<float, string> getAxisLabelY)
  {
      ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, getAxisLabelY);
  }

  private void IncreaseVisibleAmount()
  {
      ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount + 1, this.getAxisLabelX, this.getAxisLabelY);
  }

  private void DecreaseVisibleAmount()
  {
      ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);
  }


  private void SetGraphVisual(IGraphVisual graphVisual, List<float> _valueList)
  {
      ShowGraph(_valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
  }

   private void ShowGraph(List<float> valueList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
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
      
      float yMaximum = valueList[0]; //Would be used for what data you want to show value for e.g. carbon captured
      float yMinimum = valueList[0]; //Setting minimum and max values based off first element of values list

      //Set Y graph values dynamically 
      for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
      {
        float value = valueList[i];
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
      if(yDifference <= 0)
      {
        yDifference = 5f;
      }
      yMaximum = yMaximum + (yDifference * 0.2f); //Set max size with 20 percent leway
      yMinimum = yMinimum - (yDifference * 0.2f); //Set min size with 20 percent leway

      float xSize = graphWidth / (maxVisibleValueAmount + 1); //Would be used for time frame e.g. year or month
      int xIndex = 0;
      
      //LineGraphVisual lineGraphVisual = new LineGraphVisualC
      //BarChartVisual barChartVisual = new BarChartVisual(graphContainer, Color.green, .8f);
      
      //GameObject lastdotGameObject = null;

      //Setting up X axis e.g. time period
      for(int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
      {
        float xPosition = xSize + xIndex * xSize;
        float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight; //Nomralise value based off graph container
          
        //Set data point
        string tooltipText = getAxisLabelY(valueList[i]);
        graphVisualObjectList.Add(graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), xSize, tooltipText));
       
        
        //Set up X axis labels for graph values
        RectTransform labelX = Instantiate(labelTemplateX);
        labelX.SetParent(graphContainer, false);
        labelX.gameObject.SetActive(true);
        labelX.anchoredPosition = new Vector2(xPosition - 5f, 0f);
        labelX.GetComponent<Text>().text = getAxisLabelX(i);
        gameObjectList.Add(labelX.gameObject);

        //Setting the positions for dash sprites for Y axis (Yes it is supposed to be here even tho X labels are made here)
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
        //Set up Y axis labels for graph values
        RectTransform labelY = Instantiate(labelTemplateY);
        labelY.SetParent(graphContainer, false);
        labelY.gameObject.SetActive(true);
        float normalizedValue = i * 1f / serpratorCount; 
        labelY.anchoredPosition = new Vector2(-10f, (normalizedValue * graphHeight));
        labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
        gameObjectList.Add(labelY.gameObject);

        //Setting the positions for dash sprites for X axis (Yes it is supposed to be here even tho Y labels are made here)
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

private class BarChartVisual : IGraphVisual
{
   private RectTransform graphContainer;
   private Color barColor;
   float barWidthMultiplier;

   //contstructor for bar chart class
   public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier) 
   {
      //Set up propterties of bar chart, based on param values passed in 
      this.graphContainer = graphContainer;
      this.barColor = barColor;
      this.barWidthMultiplier = barWidthMultiplier;
   }

    public void CleanUp() {
        }

   public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
   {
      GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth); //Set up bar positions with gap offset
      
      BarChartVisualObject BarChartVisualObject = new BarChartVisualObject(barGameObject, barWidthMultiplier);
      BarChartVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
      

      
      return BarChartVisualObject;
      
   }
   private GameObject CreateBar(Vector2 graphPosition, float barWidth)
   { 
      GameObject gameObject = new GameObject("bar", typeof(Image));
      gameObject.transform.SetParent(graphContainer, false);
      gameObject.GetComponent<Image>().color = barColor;
      RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
      rectTransform.anchoredPosition =new Vector2(graphPosition.x, 0f);
      rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
      rectTransform.anchorMin = new Vector2(0, 0);
      rectTransform.anchorMax = new Vector2(0, 0);
      rectTransform.pivot = new Vector2(.5f, 0f);
     
      //Show tooltip of value when mouse hovers over a bar 
      Button_UI barButtonUI = gameObject.AddComponent<Button_UI>();

      return gameObject;
   }
   
      public class BarChartVisualObject : IGraphVisualObject
      {
        
        private GameObject barGameObject;
        private float barWidthMultiplier;

        public BarChartVisualObject(GameObject barGameObject, float barWidthMultiplier)
        {
          this.barGameObject = barGameObject;
          this.barWidthMultiplier = barWidthMultiplier;
        }
        public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
        {
           RectTransform rectTransform = barGameObject.GetComponent<RectTransform>();
           rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
           rectTransform.sizeDelta = new Vector2(graphPositionWidth * barWidthMultiplier, graphPosition.y);
           

           Button_UI barButtonUI = barGameObject.GetComponent<Button_UI>();
           barButtonUI.MouseOverOnceFunc += () =>
           {
             ShowToolTip_Static(tooltipText, graphPosition);
           };
      
          //Hide tooltip
           barButtonUI.MouseOutOnceFunc += () =>
           {
            HideToolTip_Static();
           };
      
          }
  
        public void CleanUp()
        {
          Destroy(barGameObject);
        }
      }
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

           dotButtonUI.MouseOverOnceFunc += () =>
           {
             ShowToolTip_Static(tooltipText, graphPosition);
           };
       
           //Hide tooltip
          dotButtonUI.MouseOutOnceFunc += () =>
          {
            HideToolTip_Static();
          };

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
