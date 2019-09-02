set -x

curl --location --request GET "http://127.0.0.1:5000/api/A" --header "Content-Type: application/json" --data "{
    \"Name\": null
}" -k -i

curl --location --request GET "http://127.0.0.1:5000/api/A" --header "Content-Type: application/json" --data "{
    \"Name\": \"something\"
}" -k -i

curl --location --request GET "http://127.0.0.1:5000/api/C" --header "Content-Type: application/json" --data "{
    \"Name\": \"something\"
}" -k -i

curl --location --request GET "http://127.0.0.1:5000/api/B" --header "Content-Type: application/json" --data "{
    \"Name\": \"something\"
}" -k -i

curl --location --request GET "http://127.0.0.1:5000/api/E" --header "Content-Type: application/json" --data "{
    \"Name\": \"something\"
}" -k -i

curl --location --request GET "http://127.0.0.1:5000/home/index" -k -i

curl --location --request GET "http://127.0.0.1:5000/not_real_page" -k -i

curl --location --request GET "http://127.0.0.1:5000/home/boom" -k -i

curl --location --request GET "http://127.0.0.1:5000/api/F" --header "Content-Type: application/json" --data "{
    \"Name\": \"something\"
}" -k -i