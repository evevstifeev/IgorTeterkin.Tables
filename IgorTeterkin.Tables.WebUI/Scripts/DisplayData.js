(function () {
    $.ajax({
        url: '/Data/GetData',
        type: 'GET',
        success: function (params) {
            const floatFormatter = new Intl.NumberFormat('ru-RU', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2,
            }),
                dateFormatOptions = {
                    day: "numeric",
                    month: "numeric",
                    year: "numeric"
                };

            var data = JSON.parse(params.generalData),
                policyGeneralData = data.generalInfo.policyGeneralData,
                naturalPersonData = data.generalInfo.policyHolder.policyParticipant.naturalPersonData,
                naturalPersonMainDocument = data.generalInfo.policyHolder.policyParticipant.naturalPersonMainDocument,
                risks = data.generalInfo.risks,
                risksDictionary = data.risksDictionary,
                $contractInfo,
                $naturalPersonData,
                totalPremium = 0,
                lastSumInsured,
                currencyRatio = (Math.round(params.currencyRatio.replace(',', '.') * 100) / 100).toFixed(2);

            $contractInfo = $('<tr>');
            $contractInfo.append('<td>' + policyGeneralData.documentBusinessNumber + '</td>');
            $contractInfo.append('<td>' + new Date(policyGeneralData.startDate).toLocaleString("ru-RU", dateFormatOptions) + '</td>');
            $contractInfo.append('<td>' + new Date(policyGeneralData.endDate).toLocaleString("ru-RU", dateFormatOptions) + '</td>');
            $contractInfo.append('</tr>');
            $('#contractInfo').append($contractInfo);

            $naturalPersonData = $('<tr>');
            $naturalPersonData.append(`<td> ${naturalPersonData.lastName} ${naturalPersonData.firstName} ${naturalPersonData.fatherName}</td>`);
            $naturalPersonData.append('<td>' + new Date(naturalPersonData.birthDate).toLocaleString("ru-RU", dateFormatOptions) + '</td>');
            $naturalPersonData.append(`<td> ${naturalPersonData.genderCode[0]}</td>`);
            $naturalPersonData.append('</tr>');
            $('#naturalPersonData').append($naturalPersonData);

            $('#naturalPersonMainDocument tr:last')
                .after(`<tr><td>Тип</td><td>${naturalPersonMainDocument.documentTypeCode}</td></tr>`);
            $('#naturalPersonMainDocument tr:last')
                .after(`<tr><td>Серия</td><td>${naturalPersonMainDocument.seria}</td></tr>`);
            $('#naturalPersonMainDocument tr:last')
                .after(`<tr><td>Номер</td><td>${naturalPersonMainDocument.number}</td></tr>`);
            $('#naturalPersonMainDocument tr:last')
                .after(`<tr><td>Код подразделения</td><td>${naturalPersonMainDocument.departmentCode}</td></tr>`);
            $('#naturalPersonMainDocument tr:last')
                .after(`<tr><td>Дата выдачи</td><td>${new Date(naturalPersonMainDocument.issueDate).toLocaleString("ru-RU", dateFormatOptions)
                    }</td></tr>`);
            $('#naturalPersonMainDocument tr:last')
                .after(`<tr>'<td>Кем выдан</td><td>${naturalPersonMainDocument.issueParty}</td></tr>`);

            if (risksDictionary.length > 0) {

                lastSumInsured = risks[risksDictionary.length - 1].sumInsured;
                for (i = 0; i < risksDictionary.length; i++) {
                    $('#risksDictionary tr:last').after(`<tr>
    <td>${risksDictionary[i].code}</td>
    <td>${risksDictionary[i].riskDescription}</td>
    <td>${floatFormatter.format(risks[i].sumInsured)}</td>
    <td>${floatFormatter.format(risks[i].premium)}</td>
    </tr>`);
                    totalPremium += risks[i].premium;
                }
            }

            $('#significantInfo tr:last').after(`<tr>
        <td>${floatFormatter.format(lastSumInsured)}</td>
        <td>${floatFormatter.format(totalPremium)}</td>
        <td>${floatFormatter.format(currencyRatio)}</td>
    </tr>`);
        },
        error: function (error) {
            // handle error here
            alert('Error')
        }
    });
    
}());