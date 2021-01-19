#terragrunt_source = "git::https://git.rockfin.com/terraform/aws-route53-tf.git//modules/aws-route53-record-tf?ref=X.X.X" # Replace X.X.X with your desire release

#-----------------------------------------------------
#-----------------Auto-Injected by HAL==--------------
#-----------------------------------------------------
# aws_account_id      = "123456789"
# aws_region          = "us-east-1"

#-----------------------------------------------------
#---------------Infrastructure Variables--------------
#-----------------------------------------------------

zone_id     = "Z3FWRREHSOU50P"
record_name = "222205-backstage-poc.sandbox.squigglelines.com"
record_type = "CNAME"
records     = ["#loadBalancerDnsName#"] #- (Required for non-alias records) A string list of records. 
# To specify a single record value longer than 255 characters such as a TXT record for DKIM, 
# add \"\" inside the Terraform configuration string (e.g. "first255characters\"\"morecharacters").
ttl = 300