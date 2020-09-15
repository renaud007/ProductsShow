using ProductsShow.Helpers;
using ProductsShow.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProductsShow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsView : ContentView
    {


        private ProductsCardsModels _viewModel;
        private readonly float _density;
        private readonly float _cardTopMargin;
        private float _cornerRadius = 60f;
        private CardState _cardState = CardState.Collapsed;
        private double _cardTopAnimPosition;
        private float _gradientTransitionY;
        private SKPaint _ProductNamePaint;
        private float _gradientHeight = 200f;

        SKColor _ProductColor;
        SKPaint _ProductPaint;
        private SKTypeface _typeface;
        private float _ProductAnimeOffsetY ;

        public ProductsView()
        {
            InitializeComponent();

            _density = (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;

            _cardTopMargin = 300f * _density;
            _cornerRadius = 30f * _density;

            if (_typeface == null)
            {
                _typeface = DependencyService.Get<IFontAssetHelper>().GetSkiaTypefaceFromAssetFont("MontserratAlternates-Bold.ttf"); 
            }
          

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.BindingContext == null) return;
            _viewModel = this.BindingContext as ProductsCardsModels;

            // because we can't bind skia drawing using the binding engine
            // we cache the paint objects when the bound character changes
           
            //initialisont la position du top de la carte
            _cardTopAnimPosition = _cardTopMargin;
            _ProductColor = Color.FromHex(_viewModel.ProductColor).ToSKColor();
            _ProductPaint = new SKPaint() { Color = _ProductColor };
            _gradientTransitionY = float.MaxValue; // cette grande valeur permet que le gradient soit en bas de l'ecran avant que l'initialisation de la variable ne debute dans l'animation
            //_ProductAnimeOffsetY = 520f * _density;

            _ProductNamePaint = new SKPaint()
            {
                Typeface = _typeface,
                IsAntialias = true,
                Color = SKColors.White,
                TextSize = 30 * _density
            };
            CardBackground.InvalidateSurface();

        }
        
        public Image ImagefromCurrentCardView
        {
            get { return ProductImage; }
        }

        private void LearnMore_TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            GoToState(CardState.Expended);
        }

        public void GoToState(CardState cardstate)
        {
            //Declencher notre animation
            if (_cardState == cardstate) return;

            MessagingCenter.Send<Message>(new Message(), cardstate.ToString());
          
            AnimateTransition(cardstate);
            _cardState = cardstate;
            productDetail.InputTransparent =  cardstate == CardState.Collapsed;
        }

        private void AnimateTransition(CardState cardstate)
        {
            //animation de l'etat de la boxview

            var parentAnimation = new Animation();
            if (cardstate == CardState.Expended)
            {
                parentAnimation.Add(0, 0.10, CreateCardAnimation(cardstate));
                parentAnimation.Add(0, 0.50, CreateImageAnimation(cardstate));
                parentAnimation.Add(0.10, 0.50, NameAnimation(cardstate));
                parentAnimation.Add(0.0, 0.25, learnMoreAnimation(cardstate));
                parentAnimation.Add(0.50, 0.75, GradientAnimation(cardstate));
                //animation des informations
                parentAnimation.Add(0.60, 0.85, new Animation(v => { ProductDetailsScroll.TranslationY = v; }, 0, -50, Easing.SpringOut));
                parentAnimation.Add(0.60, 0.85, new Animation(v => { ProductDetailsScroll.Opacity = v; }, 0 , 1, Easing.Linear));
                //animation de la barre de separation
                parentAnimation.Add(0.75, 0.85, new Animation(v => { barreDeSeparation.TranslationY = v; }, 0, -50, Easing.SpringOut));
                parentAnimation.Add(0.75, 0.85, new Animation(v => { barreDeSeparation.Opacity = v; }, 0, 1, Easing.Linear));

                //animation de logo du produit
              
                parentAnimation.Add(0.6, 0.85, new Animation(v => { ProductLogo.Opacity = v; }, 0, 1));
                parentAnimation.Add(0.6, 0.85, new Animation(v => { ProductLogo.Scale = v; }, 0, 1, Easing.SpringOut));
            }
            else
            {
                parentAnimation.Add(0.25, 0.45, CreateCardAnimation(cardstate));
                parentAnimation.Add(0.25, 0.45, CreateImageAnimation(cardstate));
                parentAnimation.Add(0.30, .50, NameAnimation(cardstate));
                parentAnimation.Add(0.35, 0.45, learnMoreAnimation(cardstate));
                parentAnimation.Add(0, 0.25, GradientAnimation(cardstate));

                //animation des informations
                parentAnimation.Add(0, 0.35, new Animation(v => { ProductDetailsScroll.TranslationY = v; }, -50, 0, Easing.SpringOut));
                parentAnimation.Add(0, 0.35, new Animation(v => { ProductDetailsScroll.Opacity = v; }, 1, 0, Easing.Linear));

                //animation de la barre de separation
                parentAnimation.Add(0, 0.10, new Animation(v => { barreDeSeparation.TranslationY = v; }, -50, 0, Easing.SpringOut));
                parentAnimation.Add(0, 0.10, new Animation(v => { barreDeSeparation.Opacity = v; }, 1, 0, Easing.Linear));

                //animation de logo du produit

                parentAnimation.Add(0, 0.15, new Animation(v => { ProductLogo.Opacity = v; }, 1, 0));
                parentAnimation.Add(0, 0.15, new Animation(v => { ProductLogo.Scale = v; }, 1, 0, Easing.SpringOut));

            }

            parentAnimation.Commit(this, cardstate.ToString(), 16, 3500);
        }

        private Animation GradientAnimation(CardState cardstate)
        {
            double start;
            double end;
            if (cardstate == CardState.Expended)
            {
                _gradientTransitionY = CardBackground.CanvasSize.Height;
                start = _gradientHeight;
                end = -_gradientTransitionY;
            }
            else
            {
                _gradientTransitionY = -_gradientHeight;
                start = _gradientTransitionY;
                end = CardBackground.CanvasSize.Height;

            }

            var gradientAnime = new Animation(v =>
            {
                _gradientTransitionY =(float)v;
                CardBackground.InvalidateSurface();
            }, start, end, Easing.Linear);
            return gradientAnime;
        }

        private Animation NameAnimation(CardState cardstate)
        {
            var NameAnimStart = cardstate == CardState.Expended ? 0 : _ProductAnimeOffsetY ;
            var NameAnimEnd = cardstate == CardState.Expended ?  _ProductAnimeOffsetY - 200: 0 ;
            var NameAnim = new Animation(v => { _ProductAnimeOffsetY = (float)v ; CardBackground.InvalidateSurface(); }, NameAnimStart, NameAnimEnd, Easing.SpringOut);
            return NameAnim;
        }

        private Animation learnMoreAnimation(CardState cardstate)
        {
            var learnMoreAnimStart = cardstate == CardState.Expended ? 0 : 200;
            var learnMoreAnimEnd = cardstate == CardState.Expended ? 200 : 0;
            var opacityStart = cardstate == CardState.Expended ? 1 : 0;
            var opacityEnd = cardstate == CardState.Expended ? 0 : 1;

            var learnMoreAnim = new Animation(v => { learnMoreButton.TranslationX = v; }, learnMoreAnimStart, learnMoreAnimEnd, Easing.SpringOut);
            learnMoreAnim.Add(0, .5, new Animation(v => { learnMoreButton.Opacity = v; }, opacityStart, opacityEnd, Easing.Linear)) ;
            
            return learnMoreAnim;
        }

        private Animation CreateImageAnimation(CardState cardstate)
        {
            var imageAnimStart = cardstate == CardState.Expended ? ProductImage.TranslationY : 0;
            var endAnimImage = cardstate == CardState.Expended ? 0 : 120;
            var imageAnim = new Animation(v => { ProductImage.TranslationY = v; }, imageAnimStart, endAnimImage, Easing.SpringOut);
            return imageAnim;
        }

        private Animation CreateCardAnimation(CardState cardstate)
        {
            var cardAnimStart = cardstate == CardState.Expended ? _cardTopMargin : _cornerRadius;
            var endAnimCard = cardstate == CardState.Expended ? -_cornerRadius : _cardTopMargin;
            var cardAnim = new Animation(v => { _cardTopAnimPosition = v; CardBackground.InvalidateSurface(); }, cardAnimStart, endAnimCard, Easing.SinInOut);
            return cardAnim;
        }

        private void CardBackground_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            if (_viewModel == null) return;

            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // draw top hero color
            canvas.DrawRoundRect(
                rect: new SKRect(3, (float)_cardTopAnimPosition, info.Width - 3, info.Height),
                r: new SKSize(_cornerRadius, _cornerRadius),
                paint: _ProductPaint);

            // work out the gradient needs to be
            var gradientRect = new SKRect(3, _gradientTransitionY, info.Width-3,
                _gradientTransitionY + _gradientHeight);

            // create the gradient
            var gradientPaint = new SKPaint() { Style = SKPaintStyle.Fill };
            gradientPaint.Shader = GetGradientShader(_ProductColor, SKColors.White);

            // draw the gradient
            canvas.DrawRect(gradientRect, gradientPaint);

            // draw the white bit
            SKRect bottomRect = new SKRect(3, _gradientTransitionY + _gradientHeight,
                info.Width-3, info.Height);
            canvas.DrawRect(bottomRect, new SKPaint() { Color = SKColors.White });

            DrawProductName(canvas);

        }




        private void DrawProductName(SKCanvas canvas)
        {
           _ProductNamePaint.Shader = GetGradientShader(SKColors.White, SKColors.Black);
            var countText = _viewModel.Name.Split(' ');  //permet de prendre mot par mot,l'espace est le seperateur 
           
            for (int i = 0; i < countText.Length; i++)
            {
                _ProductNamePaint.Shader = GetGradientShader(SKColors.White, SKColors.Black);

                var textHeigh = _ProductNamePaint.TextSize;
                canvas.DrawText(countText[i], 20, ((520f*_density) +_ProductAnimeOffsetY + textHeigh * i), _ProductNamePaint );
            }
           
        }

        private SKShader GetGradientShader(SKColor fromColor, SKColor toColor)
        {
            return SKShader.CreateLinearGradient
               (
                   start: new SKPoint(0, _gradientTransitionY),
                   end: new SKPoint(0, _gradientTransitionY + _gradientHeight),
                   colors: new SKColor[] { fromColor, toColor },
                   colorPos: new float[] { 0, 1 },
                   SKShaderTileMode.Clamp
               );
        }
    }
}