$(document).ready(function () {
    GetQuizTable();

    $('#updateformQuiz').hide();

    var add = false;
    $("#add").click(function () {
        if (add == false) {
            $(".add").slideDown("slow");
            add = true;
        } else {
            $(".add").slideUp("slow");
            add = false;
        }
    });

    var view = false;
    $("#view").click(function () {
        if (view == false) {
            $(".view").slideDown("slow");
            view = true;
        } else {
            $(".view").slideUp("slow");
            view = false;
        }
    });

    var counter = 0;
    $("#submitAnswer").click(function () {
        var getText = $("#answerText").val();

        if (getText != "") {
            var enwName = "x" + counter;
            $(".answers").append('<li id="' + enwName + 'li"> <input type="radio" name="answerGroup" id="' + enwName + '"></input> <label for="' + enwName + '" class="ansRadio">' + getText + '</label> <a href="#" onClick="deleteAnswer(' + enwName + 'li); return false;" class="delete">Delete Answer</a> </li>');
            counter++;
            $("#answerText").val("");
        }
        $('.ansRadio').click(function () {
            debugger;
            $('.ansRadio').removeClass("me");
            $(this).addClass("me");
        });
    });

    $("#cancelQuestion").click(function () {
        $(".add").slideUp(200);
        add = false;
        counter = 0;
        $(".answers").empty();
        $("#answerText").val("");
        $("#questionText").val("");
    });

    $('#btnAddQuiz').click(function () {
        var name = $('#quizName').val();

        $.ajax({
            type: 'POST',
            url: 'http://localhost:4392/DbService.asmx/AddQuiz',
            datatype: 'json',
            data: { "quizName": name },
            success: function (data) {
                $('#lblStausQuiz').html(data);
                GetQuizTable();
            }
        });
    });

    $("#updateformQuiz").submit(function (eve) {
        var quizName = $("#txtUpdateQuiz").val();

        $.ajax({
            type: 'POST',
            url: 'http://localhost:4392/DbService.asmx/UpdateQuiz',
            datatype: 'json',
            data: { "id": $("#hiddenField").val(), "quizName": quizName },
            success: function (data) {
                $('#lblStausQuiz').html(data);
                GetQuizTable();

                $("#txtUpdateQuiz").val("");

                $('#updateformQuiz').slideUp();
            }
        });

        eve.preventDefault();
    });



    $('#submitQuestion').click(function () {
        var name = $('#questionText').val();
        var quizId = $('#hdnSelectedQuiz').val();
        var list = [];
        var checkedone = "";

        $(".ansRadio").each(function (i, e) {
            debugger;
            if ($(this).hasClass("me")) {
                checkedone = $(e).html();
            } else {
                queryStr = $(e).html();
                list.push(queryStr);
            }
        });

        list.push(checkedone);

        $.ajax({
            type: 'POST',
            url: 'http://localhost:4392/DbService.asmx/AddQuestion',
            datatype: 'json',
            data: { "name": name, "quizid": quizId, "answers": JSON.stringify(list) },
            success: function (data) {
                alert(data);
            }
        });

    });
});

PopulateDropDownQuiz();

function PopulateDropDownQuiz() {

    $.ajax({
        type: 'POST',
        url: 'http://localhost:4392/DbService.asmx/GetQuizsDropDown',
        datatype: 'json',
        success: function (data) {
            var select = document.getElementById("selectQuiz");

            var returnedQuiz = JSON.parse(data);

            for (var i = 0, option; i < returnedQuiz.length; i++) {
                if (i == 0)
                {
                    $('#hdnSelectedQuiz').val(returnedQuiz[i].quizID);
                }

                option = document.createElement("option");
                option.value = returnedQuiz[i].quizID;
                option.innerText = returnedQuiz[i].quizName;
                select.appendChild(option);
            }

            select.addEventListener("change", function (event) {
                $('#hdnSelectedQuiz').val($(this).val());
            });
        }
    });
}

function deleteAnswer(id) {
    $(id).hide("slow", function () { $(id).remove(); });
}

function GetQuizTable() {
    $.ajax({
        type: 'POST',
        url: 'http://localhost:4392/DbService.asmx/GetQuizs',
        datatype: 'json',
        success: function (data) {
            $('#quizTable').html(data);

            $('.quizEditButton').click(function () {
                $.ajax({
                    type: 'POST',
                    url: 'http://localhost:4392/DbService.asmx/GetQuiz',
                    datatype: 'json',
                    data: { 'id': this.id },
                    success: function (data) {

                        var returnedQuiz = JSON.parse(data);
                        $("#txtUpdateQuiz").val(returnedQuiz.quizName);

                        $("#hiddenField").val(returnedQuiz.quizID);

                        $('#updateformQuiz').slideDown();
                        var target = $("#updateformQuiz");
                        target = target.length ? target : $hash.slice(1) + ']';
                        if (target.length) {
                            $('html,body').animate({
                                scrollTop: target.offset().top
                            }, 1000);
                            return false;
                        }
                    }
                });
            });

        }
    });
}