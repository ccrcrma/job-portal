window.addEventListener("load", function(event){
    async function ChangePublishedStatusRequest(url){
        var csrftoken = document.getElementById("RequestVerificationToken")
        var response = await fetch(url, {method:"POST", headers:{RequestVerificationToken: csrftoken.value}});
        if(response.status ===200){
            //do something
        }else {
            console.log("some error occured");
        }
        return response;
    }

    function clickAndCursor(elem){
        elem.style.cursor="pointer";
        elem.onclick = async function(event){
            var url = event.target.dataset.action;
            var response = await ChangePublishedStatusRequest(url);
            var data = await response.json();
            const{status, changeUrl} = data;
            elem.innerHTML = status;
            switch(status){
                case "Live":
                    elem.classList.remove("badge-secondary");
                    elem.classList.add("badge-success");
                    break;
                case "Draft":
                    elem.classList.remove("badge-success");
                    elem.classList.add("badge-secondary")
                    break;
            }
        }
    }
    async function PageLink(event){
        event.preventDefault();
        var response = await fetch(event.target.getAttribute("href"), {
            headers:{"Accept": "application/json"}
            });

        if(response.status == 200)
        {
            window.history.pushState("", "",response.url);
            console.log(response.url);
            var json = await response.json();
            const {items, metaData} = json
            const {baseUrl, hasPrevious, hasNext, totalPage, currentPage} = metaData;
            let tbody = document.querySelector("tbody");
            tbody.innerHTML = ""
            items.forEach(d => {
                let row = document.createElement("tr");
                let tdDate = document.createElement("td");
                var text = document.createTextNode(d.createdDate);
                tdDate.appendChild(text);

                let tdTitle = document.createElement("td");
                tdTitle.innerHTML = d.position
                let tdCompany = document.createElement("td");
                tdCompany.innerHTML = d.company;

                let tdStatus = document.createElement("td");
                const { changeUrl} = d.status
                const status = d.status.text;
                switch(status){
                    case "Live":
                        tdStatus.innerHTML = `<span data-action="${changeUrl}" class="badge status badge-success"> ${status} </span>`;
                        break;
                    case "Draft":
                        tdStatus.innerHTML = `<span data-action="${changeUrl}" class="badge status badge-secondary"> ${status} </span>`;
                        break;

                }
                let tdReadLink = document.createElement("td");
                tdReadLink.innerHTML = `<a href="${d.url}" class="btn btn-primary"> Read </a> `
                row.appendChild(tdDate);
                row.appendChild(tdTitle);
                row.appendChild(tdCompany);
                row.appendChild(tdStatus);
                row.appendChild(tdReadLink);
                tbody.appendChild(row);
            });

            var paginationLinks = document.querySelector(".pagination");
            paginationLinks.innerHTML = " ";
            var previous = document.createElement("li");
            previous.classList.add("page-item");
            if(!hasPrevious){
                previous.classList.add("disabled");
                previous.innerHTML = `<a class="page-link"><span aria-hidden="true">&laquo; </span> </a>`
            }else {
                previous.innerHTML =  `<a class="page-link" href="${baseUrl}?page=${currentPage -1}"><span aria-hidden="true">&laquo; </span> </a>`
            }
            paginationLinks.appendChild(previous);

            for(let i=1; i<=totalPage; i++){
                let pageItem = document.createElement("li");
                pageItem.classList.add("page-item");
                if(i== currentPage){
                    pageItem.classList.add("active");
                    pageItem.style.pointerEvents = "none";
                }
                pageItem.innerHTML =   `<a class="page-link" href="${baseUrl}?page=${i}"> ${i} </a>`
                paginationLinks.appendChild(pageItem);
            }

            var next = document.createElement("li");
            next.classList.add("page-item");
            if(!hasNext){
                next.classList.add("disabled");
                next.innerHTML = `<a class="page-link"><span aria-hidden="true">&raquo; </span> </a>`
            }else {
                next.innerHTML =  `<a class="page-link" href="${baseUrl}?page=${currentPage +1}"><span aria-hidden="true">&raquo; </span> </a>`
            }
            paginationLinks.appendChild(next);
        } 
        Object.values(document.querySelectorAll(".page-link")).forEach(function(elem){
            elem.onclick = PageLink;
            });
        Object.values(document.querySelectorAll(".status")).forEach(function(elem){
            clickAndCursor(elem)
    });
    }

    Object.values(document.querySelectorAll(".page-link")).forEach(function(elem){
            elem.onclick = PageLink;
        });

    Object.values(document.querySelectorAll(".status")).forEach(function(elem){
        clickAndCursor(elem)
    });
});
