using ProductsShow.Models;
using ProductsShow.ViewModels;
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
    public partial class MainPage : ContentPage
    {
        private double _heroImageTranslationY = 120;
        private double _movementFactor = 200;
        public MainPage()
        {
            this.BindingContext = new ProductsCardsVM();
            InitializeComponent();


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            mainCardsView.UserInteracted += MainCardsView_UserInteracted;
            MessagingCenter.Subscribe<Message>(this, CardState.Expended.ToString(),CardExpend) ;

        }

        private void CardExpend(Message obj)
        {
            mainCardsView.IsUserInteractionEnabled = false;

            AnimateTitle(CardState.Expended);
        }

        private void AnimateTitle(CardState cardState)
        {
            var translationY =cardState== CardState.Expended ? 0-(headersTitle.Height + headersTitle.Margin.Top) : 0 ;
            var opacity = cardState == CardState.Expended ? 0 : 1;

            var animation = new Animation();
            animation.Add(0, 1, new Animation(v => headersTitle.TranslationY = v, headersTitle.TranslationY, translationY)); 
            animation.Add(0, 1, new Animation(v => headersTitle.Opacity = v, headersTitle.Opacity, opacity));
            animation.Commit(this, "Title animation", 16, 300 );
        }

        private void BackButton_Tapped(object sender, EventArgs e)
        {
            // animation du headertitle
            AnimateTitle(CardState.Collapsed);

            //mis à jour de la variable dans le produductsView.xaml.cs
            ((ProductsView)mainCardsView.CurrentView).GoToState(CardState.Collapsed);

            mainCardsView.IsUserInteractionEnabled = true; // permettre à nouveau le swipping
        }

       
        private void MainCardsView_UserInteracted(PanCardView.CardsView view,
            PanCardView.EventArgs.UserInteractedEventArgs args)
        {
            

            if (args.Status == PanCardView.Enums.UserInteractionStatus.Running)
            {
                // get the front card
                var carteEnCours = mainCardsView.CurrentView as ProductsView;

                // work out what percent the swipe is at
                var percentFromCenter = Math.Abs(args.Diff / this.Width);

                // adjust scale when panning
                if ((percentFromCenter > 0) && (carteEnCours.Scale == 1))
                    carteEnCours.ScaleTo(.95, 50);

                // update elements based on swipe position
                AnimateFrontCardDuringSwipe(carteEnCours, percentFromCenter);

                // get the next card on the stack, which is the one coming into view
                var nextCard = mainCardsView.CurrentBackViews.First() as ProductsView;

                // update elements based on swipe position
                AnimateIncomingCardDuringSwipe(nextCard, percentFromCenter);
            }

            if (args.Status == PanCardView.Enums.UserInteractionStatus.Ended ||
                args.Status == PanCardView.Enums.UserInteractionStatus.Ending)
            {
                // at the end of dragging we need to make sure card is reset
                var card = mainCardsView.CurrentView as ProductsView;
                AnimateFrontCardDuringSwipe(card, 0);
                card.ScaleTo(1, 50);
            }
        }

        private void AnimateFrontCardDuringSwipe(ProductsView card, double percentFromCenter)
        {
            // opacity of the maincard during swipe
            mainCardsView.CurrentView.Opacity = LimitToRange((1 - (percentFromCenter)) * 2, 0, 1);

            // scaling on the main card during swipe
            card.ImagefromCurrentCardView.Scale = LimitToRange((1 - (percentFromCenter) * 1.5), 0, 1);

            // y offset of image during swipe
            card.ImagefromCurrentCardView.TranslationY = _heroImageTranslationY + (_movementFactor * percentFromCenter);

            // adjust opacity of image
            card.ImagefromCurrentCardView.Opacity = LimitToRange((1 - (percentFromCenter)) * 1.5, 0, 1); ;
        }

        private void AnimateIncomingCardDuringSwipe(ProductsView nextCard, double percentFromCenter)
        {
            // opacity fading in
            nextCard.ImagefromCurrentCardView.Opacity = LimitToRange(percentFromCenter * 1.5, 0, 1);

            // scaling in
            nextCard.ImagefromCurrentCardView.Scale = LimitToRange(percentFromCenter * 1.1, 0, 1);

            var offset = _heroImageTranslationY + (_movementFactor * (1 - (percentFromCenter * 1.1)));
            nextCard.ImagefromCurrentCardView.TranslationY = LimitToRange(offset, _heroImageTranslationY, _heroImageTranslationY + _movementFactor);
        }

        public double LimitToRange(double value, double InclusiveMinimum, double InclusiveMaximum)
        {
            if (value >= InclusiveMinimum)
            {
                if (value <= InclusiveMaximum)
                {
                    return value;
                }
                return InclusiveMaximum;
            }
            return InclusiveMinimum;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing(); mainCardsView.UserInteracted -= MainCardsView_UserInteracted;
            MessagingCenter.Unsubscribe<Message>(this, CardState.Expended.ToString());
        }

         
    }
}