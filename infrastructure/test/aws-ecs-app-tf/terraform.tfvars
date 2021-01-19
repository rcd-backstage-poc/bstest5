# terragrunt_source = "git::https://git.rockfin.com/terraform/aws-ecs-app-tf.git?ref=X.X.X" # Substitute X with the latest release

# ---------------------------------------------------------------------------------------------------------------------
# Required variables for AWS
# ---------------------------------------------------------------------------------------------------------------------

#aws_region     = "us-east-2" # set as env var TF_VAR_aws_region

development_team_email        = "mck222@gmail.com"
infrastructure_team_email     = "mck222@gmail.com"
infrastructure_engineer_email = "mck222@gmail.com"

# ---------------------------------------------------------------------------------------------------------------------
# Standard Module Required Variables
# ---------------------------------------------------------------------------------------------------------------------


app_id           = "222205"
application_name = "bstest5" # Alphanumeric characters, lowercase ONLY, 16 characters MAX. DO NOT EXCEED
environment      = "test"        # Must match the environment of the cluster, this is how it determines cluster placement

# ---------------------------------------------------------------------------------------------------------------------
# Infrastructure Tags
# ---------------------------------------------------------------------------------------------------------------------

app_tags = {
  hal-app-id = "222205"
}

# ---------------------------------------------------------------------------------------------------------------------
# Infrastructure Variables
# ---------------------------------------------------------------------------------------------------------------------


vpc_id      = "vpc-0b20139ab8e425867"
subnet_type = "public" # Type of subnet to build your ALB in. Valid values are public, private, private-pers. Comment this line if using subnet_ids variable
subnet_ids              = ["subnet-0b12df0a0721b2f31", "subnet-09bf74d8417d8a508", "subnet-0b20669213a96052c"] # Uncomment this to provide custom VPC Subnets that the Lambda proxy will proxy into.
desired_number_of_tasks = 1 # Recommended values: 1 for dev/test, 3(or however many subnets you have) for beta/prod
min_number_of_tasks     = 1 # Recommended values: 1 for dev/test, 3(or however many subnets you have) for beta/prod
max_number_of_tasks     = 1 # Recommended values: 1 for dev/test, 9(or however many subnets you have) for beta/prod

ecs_cluster_name = "grafana-sandbox-ecs"
use_fargate = true
email_alerts = false
use_kraken_parameter_store = true

# ---------------------------------------------------------------------------------------------------------------------
# Load Balancer Variables
# ---------------------------------------------------------------------------------------------------------------------

use_tls                            = true
create_load_balancer               = true
acm_certificate                    = "#certificate_arn#"
load_balancer_type                 = "public"                                                 # Should be set to "private" for most internally hosted API's, then fronted with an API GW; public for user-facing websites
load_balancer_subnet_ids           = ["subnet-09040ac2efded095d", "subnet-03dc7fde006535518", "subnet-08aab82c4b582fe3e"] # Should be private subnets if load_balancer_type is "private"; public subnets if load_balancer_type is "public"
allow_connections_from_cidr_blocks = ["12.165.188.0/24", "162.252.136.0/21"]                  # What IP space to lock down the load balancer to, default is QL IP space

# ---------------------------------------------------------------------------------------------------------------------
# Health Check Variables
# ---------------------------------------------------------------------------------------------------------------------

health_check_path = "/healthcheck" # Health check path for your application

# ---------------------------------------------------------------------------------------------------------------------
# AutoScaling Variables
# ---------------------------------------------------------------------------------------------------------------------

use_auto_scaling = false # Recommended values: false for dev/test, true for beta/prod
use_blue_green   = false # Recommended values: false for dev/test/beta, true for prod
