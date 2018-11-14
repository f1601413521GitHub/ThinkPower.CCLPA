
function callAjax(type, url, data) {

    let resultInfo = { success: false, errorMsg: null, info: null, isJson: false };

    $.ajax({
        type: type,
        url: url,
        data: data,
        cache: false,
        async: false,
        success: function (result) {

            resultInfo.success = true;
            resultInfo.info = result;

            if (checkIsJson(result)) {

                resultInfo.isJson = true;
            }
        },
        error: function () {
            resultInfo.errorMsg = '系統發生錯誤，請於上班時段來電客服中心0800-123-456，造成不便敬請見諒。';
        }
    });


    return resultInfo;
}

function checkIsJson(result) {

    if (typeof result !== 'string') {
        result = JSON.stringify(result);
    }

    try {
        JSON.parse(result);
        return true;

    } catch (e) {
        return false;
    }
}