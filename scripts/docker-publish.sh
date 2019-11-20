# DOCKER_ENV=''
DOCKER_TAG=''
# DOCKER_ENV=docker

case "$TRAVIS_BRANCH" in
  "master")
    # DOCKER_ENV=production
    DOCKER_TAG=latest
    ;;
  "develop")
    # DOCKER_ENV=development
    DOCKER_TAG=dev
    ;;    
esac

echo "login with tag $DOCKER_TAG"
docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD

echo "docker-build"
docker build -f ./src/StockAdvisor.Api/Dockerfile -t stock-advisor:$DOCKER_TAG ./src/StockAdvisor.Api --no-cache

echo "docker-tag"
docker tag stock-advisor:$DOCKER_TAG $DOCKER_USERNAME/stock-advisor:$DOCKER_TAG

echo "docker-push"
docker push $DOCKER_USERNAME/stock-advisor:$DOCKER_TAG