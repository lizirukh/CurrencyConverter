// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    console.log("JQuery works fine");

    var previousOriginalValue;
    $("#original").on('focus', function () {

        previousOriginalValue = this.value;
    }).change(function () {

        let originalValue = $('#original').val();
        let destinationValue = $('#destination').val();

        if (originalValue === destinationValue) {

            $(`#destination option[value=${previousOriginalValue}]`).prop('selected', true);
            $(`#original option[value=${destinationValue}]`).prop('selected', true);

            // console.log("inside if satatement");
        }

    });

    var previousDestinationValue;
    $("#destination").on('focus', function () {

        previousDestinationValue = this.value;
    }).change(function () {

        let originalValue = $('#original').val();
        let destinationValue = $('#destination').val();
        if (originalValue === destinationValue) {

            $(`#destination option[value=${originalValue}]`).prop('selected', true);
            $(`#original option[value=${previousDestinationValue}]`).prop('selected', true);

            // console.log("inside if satatement");
        }

    });


    function customTrim(txt) {
        if (txt === undefined || txt === null)
            return null;
        return txt.trim();
    }

    function validateAmount(txt) {

        if (txt === null)
            return false;

        let len = txt.length;
        if (len === 0 || len > 15)
            return false;
        
        return !isNaN(parseFloat(txt)) && !isNaN(txt - 0);
    }

    $("#currencyForm").submit(function (event) {
        event.preventDefault();

        let originalValue = $('#original').val();
        let destinationValue = $('#destination').val();
        let amount = customTrim($('#amountID').val());

        if (validateAmount(amount)) {
            $('#amountMessages').html('');
            $('#submitBtn').prop("disabled", true);
            $('#submitBtn').html('Converting ...');
            const postData = {
                originalCode: originalValue,
                destinationCode: destinationValue,
                amount: Number.parseFloat(amount).toFixed(2)
            };

            $.post('/Currency/Converter', postData)
                .done(function (response) {

                    console.log(response);
                   // console.log(response.data.result);
                    let template = ``;
                    template += `1 ${response.data.originalCode} = ${response.data.originalRate} ${response.data.destinationCode} <br>`;
                    template += `1 ${response.data.destinationCode} = ${response.data.destinationRate} ${response.data.originalCode} <br>`;
                    template += `Converted amount: ${response.data.result} <br>`;
                    template += `Conversion date: ${response.data.conversionDate} <br>`;
                    $('#converterMessages').html(template);

                }).fail(function (err) {
                    console.log(`err = ${err}`);
                }).always(function () {
                    $('#submitBtn').prop("disabled", false);
                    $('#submitBtn').html('Submit');
                });

        }
        else {
            $('#amountMessages').html('Please, enter valid amount:');
        }

    });

    //save currency
    var dayInMilliseconds = 1000 * 60 * 60 * 24;
    var intervalInMilliseconds = dayInMilliseconds;
    setInterval(function () {
        $.post('/Currency/SaveCurrencyInterval').done(() => {

        });
    }, intervalInMilliseconds);

})
