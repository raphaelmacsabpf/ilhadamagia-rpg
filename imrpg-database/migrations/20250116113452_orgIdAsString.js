
exports.up = function (knex) {
    return knex.schema
      .alterTable('imtb_org', function (table) {
        table.string('Id').notNullable().alter();
      })
      .then(() =>
        knex.raw('ALTER TABLE imtb_org MODIFY COLUMN Id VARCHAR(255) NOT NULL;') 
      )
      .then(() =>
        knex.schema.alterTable('imtb_player_orgs', function (table) {
          table.string('OrgId').notNullable().alter();
        })
      );
  };
  
  exports.down = function (knex) {
    return knex.schema
      .alterTable('imtb_org', function (table) {
        table.integer('Id').unsigned().notNullable().alter();
      })
      .then(() =>
        knex.raw(
          'ALTER TABLE imtb_org MODIFY COLUMN Id INT UNSIGNED NOT NULL AUTO_INCREMENT;'
        )
      )
      .then(() =>
        knex.schema.alterTable('imtb_player_orgs', function (table) {
          table.integer('OrgId').notNullable().alter();
        })
      );
  };
